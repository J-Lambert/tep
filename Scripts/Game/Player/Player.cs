namespace TEP.Game
{
	public partial class Player : Node2D
	{
		[Export] public Texture2D PlayerSprite;

		// Depending on sprite's size, needs to be offset so it aligns with the bottom of tiles.
		[Export] public Vector2 PlayerSpriteOffset = Vector2.Zero;

		// Size of each tile in pixels.
		[Export] public int TileSize = 16;
		[Export] public TileMapLayer BlockingLayer;

		// Coordinates of the player's current position in the tile grid.
		public Vector2I GridPosition = Vector2I.Zero;

		// Reference to the player's sprite node itself.
		private Sprite2D _sprite;

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

        public override void _UnhandledInput(InputEvent @event)
        {
            HandleInput();
        }

		// Snaps player position to the grid.
		private void SnapToGrid()
		{
			// Ensures player is centered in the tile no matter the size.
			Position = (Vector2)(GridPosition * TileSize) + new Vector2(TileSize / 2, TileSize / 2);
		}

		private void HandleInput()
		{
			Vector2I direction = Vector2I.Zero;

			if (Input.IsActionJustPressed("move_up"))
			{
				direction = Vector2I.Up;
			}
			else if (Input.IsActionJustPressed("move_down"))
			{
				direction = Vector2I.Down;
			}
			else if (Input.IsActionJustPressed("move_left"))
			{
				direction = Vector2I.Left;
			}
			else if (Input.IsActionJustPressed("move_right"))
			{
				direction = Vector2I.Right;
			}

			if (direction != Vector2I.Zero)
			{
				TryMoveInDirection(direction);
			}
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

		// Checks if a particular tile is in the 'Blocking' TileMapLayer.
		private bool IsTileBlocked(Vector2I gridPos)
		{
			TileData tileData = BlockingLayer.GetCellTileData(gridPos);

			return tileData != null;
		}
	}
}