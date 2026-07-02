using System.Collections.Generic;

namespace TEP.Game.Combat.UI
{
	// Draws the unit's movement path using an autotile.
	public partial class UnitPath : TileMapLayer
	{
		/* Caches a path found by _pathfinder so it can be reused from the combat board. If unit movement
		   is confirmed, the path is passed to the unit's WalkAlong() function. */
		public List<Vector2I> CurrentPath = [];

		private PathFinder _pathfinder;
		private CombatBoard _board;
		private readonly Godot.Collections.Array<Vector2I> _terrainBuffer = [];

        public override void _Ready()
        {
            _board = GetParent<CombatBoard>();
        }

		// Called every time a unit is selected.
		public void Initialize(List<Vector2I> walkableTiles)
		{
			_pathfinder = new PathFinder(_board, walkableTiles);
		}

		// Finds & draws the path between startTile & endTile.
		public void DrawPath(Vector2I startTile, Vector2I endTile)
		{
			// Clears any tiles on the tilemap, then finds the path.
			Clear();

			if (_pathfinder == null)
			{
				return;
			}

			CurrentPath = _pathfinder.CalculatePointPath(startTile, endTile);

			if (CurrentPath.Count == 0)
			{
				return;
			}

			if (CurrentPath[0] != startTile)
			{
				var fixedPath = new List<Vector2I> {startTile};
				fixedPath.AddRange(CurrentPath);
				CurrentPath = fixedPath;
			}

			_terrainBuffer.Clear();

			// Modified array to work with Tileset Terrains system.
			foreach (Vector2I point in CurrentPath)
			{
				_terrainBuffer.Add(point);
			}

			// Draws the tile path.
			SetCellsTerrainConnect(_terrainBuffer, 0, 0);
		}

		// Stops drawing, clearing the drawn path.
		public void Stop()
		{
			Clear();
			CurrentPath = [];
		}
	}
}
