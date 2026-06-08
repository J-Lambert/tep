namespace TEP.Game
{
	[GlobalClass]
	public partial class Player : Node2D
	{
		[Export] public Texture2D PlayerSprite;

		// Depending on sprite's size, needs to be offset so it aligns with the bottom of tiles.
		[Export] public Vector2 PlayerSpriteOffset = Vector2.Zero;

		// Size of each tile in pixels.
		[Export] public int TileSize = 16;
		[Export] public TileMapLayer BlockingLayer;

		// Time until player can move again, in seconds.
		[Export] public float MoveCooldown = 0.1f;

		// Coordinates of the player's current position in the tile grid.
		public Vector2I GridPosition = Vector2I.Zero;

		// Used to calculate the center of a grid tile in pixels, on the screen.
		private Vector2I _halfTileSize => new(TileSize / 2, TileSize / 2);

		// Reference to the player's sprite node itself.
		private Sprite2D _sprite;
		private float _moveTimer = 0;

        public override void _Ready()
		{
			_sprite = GetNode<Sprite2D>("Sprite");

			if (PlayerSprite != null)
			{
				_sprite.Texture = PlayerSprite;
			}

			_sprite.Position = PlayerSpriteOffset;

			// Converts current world position to grid position.
			GridPosition = new Vector2I(Mathf.RoundToInt(Position.X / TileSize), Mathf.RoundToInt(Position.Y / TileSize));
			SnapToGrid();
		}

        public override void _Process(double delta)
        {
			_moveTimer -= (float)delta;

			if (_moveTimer > 0)
			{
				return;
			}

			Vector2I dir = GetInputDirection();

			if (dir != Vector2I.Zero)
			{
				TryMoveInDirection(dir);
				_moveTimer = MoveCooldown;
			}
        }

		public Vector2I CalculateGridCoordinates(Vector2 boardPos)
		{
			return (Vector2I)(boardPos / TileSize).Floor();
		}

		public Vector2 CalculateMapPosition(Vector2 gridPos)
		{
			return gridPos * TileSize + _halfTileSize;
		}

		private Vector2I GetInputDirection()
		{
			int x = 0;
			int y = 0;
			Vector2I direction = Vector2I.Zero;

			if (Input.IsActionPressed("move_up"))
			{
				y -= 1;
			}
			if (Input.IsActionPressed("move_down"))
			{
				y += 1;
			}
			if (Input.IsActionPressed("move_left"))
			{
				x -= 1;
			}
			if (Input.IsActionPressed("move_right"))
			{
				x += 1;
			}

			if (y != 0)
			{
				return new Vector2I(0, y);
			}

			if (x != 0)
			{
				return new Vector2I(x, 0);
			}

			return Vector2I.Zero;
		}

		// Checks if a particular tile is in the 'Blocking' TileMapLayer.
		private bool IsTileBlocked(Vector2I gridPos)
		{
			TileData tileData = BlockingLayer.GetCellTileData(gridPos);

			if (tileData == null)
			{
				return false;
			}

			return (bool)tileData.GetCustomData("walkable") == false;
		}

		// Snaps player position to the grid.
		private void SnapToGrid()
		{
			// Ensures player is centered in the tile no matter the size.
			Position = (Vector2)(GridPosition * TileSize) + new Vector2(TileSize / 2, TileSize / 2);
		}

		// Attempts to move the player using the 'direction' vector if no blocking tiles are in its way.
		private void TryMoveInDirection(Vector2I direction)
		{
			Vector2I targetPosition = GridPosition + direction;

			// Check if the target tile exists in the blocking layer.
			if (IsTileBlocked(targetPosition))
			{
				return;
			}

			GridPosition = targetPosition;
			SnapToGrid();
		}
	}
}