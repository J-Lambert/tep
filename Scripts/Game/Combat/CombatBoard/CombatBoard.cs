using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEP.Game.Combat.UI;
using TEP.Game.Core;

namespace TEP.Game.Combat
{
	/* Represents a grid of tiles with its size, the size of each tile in pixels, & some helper functions to calculate
	   & convert coordinates. Meant to be shared between game objects that need access to those values.*/
	public partial class CombatBoard : Node2D
	{
		[Export] public PackedScene TileScene;
		[Export] public Texture2D MainTileTexture;
		[Export] public Texture2D AltTileTexture;

		// The grid's size in rows & columns.
		[Export(PropertyHint.Range, "1,500")] public Vector2I GridSize = new(9, 9);
		// The size of a tile in pixels.
        [Export(PropertyHint.Range, "1,1024")] public Vector2I TileSize = new(16, 16);

		// Margin around board in terms of a float percentage (0.1 = 10%)
		[Export(PropertyHint.Range, "-3.0,3.0,0.01")] public float BoardPaddingX = 0.1f;
		[Export(PropertyHint.Range, "-3.0,3.0,0.01")] public float BoardPaddingY = 0.1f;

		/* Each key-value pair in the dictionary represents a unit. The key is the position in grid
		   coordinates, while the value is a reference to the unit. */
		private Dictionary<Vector2I, Unit> _units = [];
		private Unit _activeUnit;

		// Array of all the tiles the _activeUnit can move to.
		private List<Vector2I> _walkableTiles = [];

		private UnitOverlay _unitOverlay;
		private UnitPath _unitPath;
		private Node2D _tileContainer;
		private Node2D _unitContainer;
		private Camera2D _camera;

		// Used to calculate the center of a grid tile in pixels.
        private Vector2 _halfTileSize => TileSize / 2;
		private CombatTile[,] _tiles;


		public override void _Ready()
		{
			_unitOverlay = GetNode<UnitOverlay>("UnitOverlay");
			_unitPath = GetNode<UnitPath>("UnitPath");
			_tileContainer = GetNode<Node2D>("TileContainer");
			_unitContainer = GetNode<Node2D>("UnitContainer");
			_camera = GetNode<Camera2D>("CombatCamera");

			GenerateBoard();
			SetupCamera();

			foreach (Node child in _unitContainer.GetChildren())
			{
				if (child is Unit unit)
				{
					RegisterUnit(unit);
				}
			}
		}

        public override void _UnhandledInput(InputEvent @event)
        {
            if (_activeUnit != null && @event.IsActionPressed("ui_cancel"))
			{
				DeselectActiveUnit();
				ClearActiveUnit();
			}
        }

		// Clears the reference to the _activeUnit & corresponding walkable tiles.
		private void ClearActiveUnit()
		{
			_activeUnit = null;
			_walkableTiles.Clear();
		}

		// Deselects the active unit, clearing the tiles overlay & move path.
		private void DeselectActiveUnit()
		{
			if (_activeUnit == null)
			{
				return;
			}

			_activeUnit.IsSelected = false;

			_unitOverlay.Clear();
			_unitPath.Stop();
		}

		// Returns an array with all the coordinates of walkable tiles based on maxDistance.
		private List<Vector2I> FloodFill(Vector2I startTile, int maxDistance)
		{
			// Output list.
			List<Vector2I> result = [];
			HashSet<Vector2I> visited = [];

			// Every tile to apply the flood fill algorithm to is stored in the stack.
			Stack<Vector2I> stack = new();
			stack.Push(startTile);

			while (stack.Count > 0)
			{
				Vector2I current = stack.Pop();

				// Conditions to fill further:
				// 1. Not past the grid's limits.
				if (IsWithinBounds(current))
				{
					continue;
				}

				// 2. Tile hasn't already been visited & filled.
				if (!visited.Add(current))
				{
					continue;
				}

				// 3. Within the maxDistance # of tiles.
				Vector2I difference = (current - startTile).Abs();
				int distance = difference.X + difference.Y;
				if (distance > maxDistance)
				{
					continue;
				}

				// If all conditions are met, 'current' tile is stored in the output array.
				result.Add(current);

				// If current tile's neighbors are not occupied/already visited, they are added to the stack.
				foreach (Vector2I direction in GridDirections.Cardinal)
				{
					Vector2I coords = current + direction;

					if (IsOccupied(coords) && coords != startTile)
					{
						continue;
					}
					if (visited.Contains(coords))
					{
						continue;
					}

					stack.Push(coords);
				}
			}

			return result;
		}

		private void GenerateBoard()
		{
			// Initializes 2D array of tiles with a set width & height.
			_tiles = new CombatTile[GridSize.X, GridSize.Y];

			for (int y = 0; y < GridSize.Y; y++)
			{
				for (int x = 0; x < GridSize.X; x++)
				{
					CombatTile tile = TileScene.Instantiate<CombatTile>();

					Vector2 tilePosInBoard = new Vector2(x, y);

					_tileContainer.AddChild(tile);
					tile.Position = TileToWorld(tilePosInBoard);

					bool mainTile = (x + y) % 2 == 0;

					tile.Initialize(new Vector2I(x, y), mainTile ? MainTileTexture : AltTileTexture);

					_tiles[x, y] = tile;
				}
			}
		}

		public CombatTile GetTile(Vector2I position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= GridSize.X || position.Y >= GridSize.Y)
			{
				return null;
			}

			return _tiles[position.X, position.Y];
		}

		// Returns an array of tiles a given unit can walk to using the flood fill algorithm.
		public List<Vector2I> GetWalkableTiles(Unit unit)
		{
			return FloodFill(unit.Tile, unit.MoveRange);
		}

		// Returns true if the tile is occupied by a unit.
		public bool IsOccupied(Vector2I tile)
		{
			return _units.ContainsKey(tile);
		}

		// Updates the _units dictionary with the target position for the unit & asks the _activeUnit to walk to it.
		private async Task MoveActiveUnit(Vector2I newTile)
		{
			if (IsOccupied(newTile) || !_walkableTiles.Contains(newTile))
			{
				return;
			}

			Vector2I oldTile = _activeUnit.Tile;

			List<Vector2I> path = new(_unitPath.CurrentPath);

			DeselectActiveUnit();

			_activeUnit.WalkAlong(path);

			await ToSignal(_activeUnit, Unit.SignalName.UnitMoveFinished);

			_units.Remove(oldTile);
			_activeUnit.Tile = newTile;
			_units[newTile] = _activeUnit;

			ClearActiveUnit();
		}

		public void RegisterUnit(Unit unit)
		{
			_units[unit.Tile] = unit;
		}

		/* Selects the unit in the tile, if there is one. Sets it as the _activeUnit & draws its walkable tiles & move
		   path. The board reacts to signals emitted by calling functions that select & move the unit. */
		private void SelectUnit(Vector2I tile)
		{
			// Return early from the function if the unit's not registered in the tile.
			if (!_units.ContainsKey(tile))
			{
				return;
			}

			_activeUnit = _units[tile];
			_activeUnit.IsSelected = true;

			_walkableTiles = GetWalkableTiles(_activeUnit).ToList();
			_unitOverlay.Draw(_walkableTiles);
			_unitPath.Initialize(_walkableTiles);
		}

		private void SetupCamera()
		{
			_camera.Enabled = true;

			float boardPixelWidth = GridSize.X * TileSize.X;
			float boardPixelHeight = GridSize.Y * TileSize.Y;

			Vector2 viewportSize = GetViewportRect().Size;

			float zoomX = viewportSize.X / boardPixelWidth;
			float zoomY = viewportSize.Y / boardPixelHeight;

			// Takes the smaller dimension in cases of a rectangular board.
			float zoom = Mathf.Min(zoomX, zoomY);

			// Center camera on combat board.
			_camera.Position = new Vector2(boardPixelWidth / 2.0f, boardPixelHeight / 2.0f);

			_camera.Zoom = new Vector2(zoom * (1 - BoardPaddingX), zoom * (1 - BoardPaddingY));
		}

		public void UnregisterUnit(Unit unit)
		{
			if (_units.TryGetValue(unit.Tile, out Unit existing) && existing == unit)
			{
				_units.Remove(unit.Tile);
			}
		}

		// Updates the path's drawing if there's an active & selected unit.
		private void OnCursorMoved(Vector2I newTile)
		{
			if (_activeUnit != null && _activeUnit.IsSelected)
			{
				if (!_walkableTiles.Contains(newTile))
				{
					_unitPath.Stop();
					return;
				}

				_unitPath.DrawPath(_activeUnit.Tile, newTile);
			}
		}

		// Selects/moves a unit based on where the cursor is.
		private async void OnCursorAcceptPressed(Vector2I tile)
		{
			// This interaction either causes a unit to be selected or a move order to be executed.
			if (_activeUnit == null)
			{
				SelectUnit(tile);
			}
			else if (_activeUnit.IsSelected)
			{
				await MoveActiveUnit(tile);
			}
		}

		///////////////////////////////////////////////////////////////
		/// Helper functions
		// Converts 2D tile coordinates into a 1D array index.
        public int AsIndex(Vector2I tile)
        {
            return tile.X + GridSize.X * tile.Y;
        }

        // Returns the coordinates of the tile on the grid given a position on the map.
        public Vector2I WorldToTile(Vector2 worldPos)
        {
            return (Vector2I)(worldPos / TileSize).Floor();
        }

        // Returns the position of a tile's center in pixels.
        public Vector2 TileToWorld(Vector2 tilePosInBoard)
        {
            return tilePosInBoard * TileSize + _halfTileSize;
        }

        // Makes the gridPosition fit within the grid's bounds by clamping each of the vector's values individually.
        public Vector2I Clamp(Vector2I gridPos)
        {
            Vector2I clampedPos = gridPos;

            clampedPos.X = Mathf.Clamp(gridPos.X, 0, GridSize.X - 1);
            clampedPos.Y = Mathf.Clamp(gridPos.Y, 0, GridSize.Y - 1);

            return clampedPos;
        }

        /* Returns true if the passed in tile coordinates are within the grid.
           Used to ensure the cursor/units can never go past the grid's limits. */
        public bool IsWithinBounds(Vector2I tileCoords)
        {
            bool insideX = tileCoords.X >= 0 && tileCoords.X < GridSize.X;
            bool insideY = tileCoords.Y >= 0 && tileCoords.Y < GridSize.Y;

            return insideX && insideY;
        }
	}
}