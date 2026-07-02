namespace TEP.Game.Combat.UI
{
	// Player-controlled cursor used to navigate the game grid & to select & move units.
	public partial class Cursor : Node2D
	{
		// Emitted when clicking on the currently hovered tile, or when pressing "ui_accept".
		[Signal] public delegate void AcceptPressedEventHandler(Vector2 tile);

		// Emitted when the cursor is moved to a new tile.
		[Signal] public delegate void CursorMovedEventHandler(Vector2 newTile);

		// Coordinates of the current tile the cursor is hovering.
		public Vector2I Tile
		{
			get => _tile;
			set => SetTile(value);
		}

		private Vector2I _tile = Vector2I.Zero;
		private CombatBoard _board;

		// Snap cursor position to center of tile once it enters the scene tree.
		public override void _Ready()
		{
			_board = GetParent<CombatBoard>();
			_board.TileToWorld(_tile);
		}

        public override void _UnhandledInput(InputEvent @event)
        {
            // If the user moves the mouse, input is captured & node's tile is updated.
			if (@event is InputEventMouseMotion motion)
			{
				// Convert mouse position to global coordinates to compensate for Camera2D zoom/position.
				Vector2 worldPos = GetGlobalMousePosition();
				Tile = _board.WorldToTile(worldPos);
			}
			// If tile is already being hovered & is clicked/"ui_accept" (Enter) key is pressed, interact with that tile.
			else if (@event.IsActionPressed("click") || @event.IsActionPressed("ui_accept"))
			{
				EmitSignal(SignalName.AcceptPressed, Tile);
				GetViewport().SetInputAsHandled();
			}
        }

		private void SetTile(Vector2I value)
		{
			// Clamp tile coordinates & ensure it's not outside the grid's boundaries.
			Vector2I newTile = _board.Clamp(value);
			if (newTile.Equals(_tile))
			{
				return;
			}
			_tile = newTile;

			// If new tile is moved to, update cursor's position & emit a signal.
			Position = _board.TileToWorld(_tile);
			EmitSignal(SignalName.CursorMoved, _tile);
		}
	}
}
