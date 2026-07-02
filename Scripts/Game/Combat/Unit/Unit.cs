using System.Collections.Generic;

namespace TEP.Game.Combat
{
	/* Represents a unit on the combat board. The board manages the Unit's position.
	   The unit itself is only a visual representation. */
	public partial class Unit : Node2D
	{
		/* Emitted when the unit reaches the end of a path along which it was moving. Used to notify
		   the combat board that a unit reached its destination, so gameplay can continue.*/
		[Signal] public delegate void UnitMoveFinishedEventHandler();

		// Distance to which the unit can walk in tiles.
		[Export(PropertyHint.Range, "0, 500")] public int MoveRange = 6;

		// Texture representing the unit.
		[Export] public Texture2D UnitTexture;

		// Unit's move speed in pixels, when it's moving along a path.
		[Export] public float MoveSpeed = 600.0f;

		private CombatBoard _board;

		// Coordinates of the board's tile the unit is on.
		public Vector2I Tile
		{
			get => _tile;
			set => SetTile(value);
		}

		// Toggles the "selected" animation on the unit.
		public bool IsSelected
		{
			get => _isSelected;
			set => SetIsSelected(value);
		}

		// Toggles processing for the unit.
		private bool IsWalking
		{
			get => _isWalking;
			set => SetIsWalking(value);
		}

		private Vector2I _tile = Vector2I.Zero;
		private bool _isSelected = false;
		private bool _isWalking = false;

		private Queue<Vector2> _waypoints = new();
		private Vector2 _targetPosition;

		private Sprite2D _sprite;
		private AnimationPlayer _animPlayer;

		public void Initialize(CombatBoard board)
		{
			_board = board;
		}

		public override void _Ready()
		{
			SetProcess(false);

			_sprite = GetNode<Sprite2D>("Sprite");
			_animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

			if (UnitTexture != null)
			{
				_sprite.Texture = UnitTexture;
			}
		}

		public override void _Process(double delta)
		{
			if (!IsWalking)
			{
				return;
			}

			float speed = MoveSpeed * (float)delta;

			Vector2 toTarget = _targetPosition - Position;

			// Changes direction sprite is facing based on direction of movement.
			if (Mathf.Abs(toTarget.X) > Mathf.Abs(toTarget.Y))
			{
				_sprite.FlipH = toTarget.X < 0;
			}

			Position = Position.MoveToward(_targetPosition, speed);

			if (Position == _targetPosition)
			{
				Position = _targetPosition;

				if (_waypoints.Count == 0)
				{
					IsWalking = false;
					EmitSignal(SignalName.UnitMoveFinished);
					return;
				}

				_targetPosition = _waypoints.Dequeue();
			}
		}

		private void SetIsSelected(bool value)
		{
			_isSelected = value;

			if (_isSelected)
			{
				_animPlayer.Play("selected");
			}
			else
			{
				_animPlayer.Play("idle");
			}
		}

		private void SetIsWalking(bool value)
		{
			_isWalking = value;

			SetProcess(_isWalking);
		}

		private void SetTile(Vector2I value)
		{
			_tile = _board.Clamp(value);
		}

		// Starts unit moving along the path, an array of board coordinates converted to map coordinates.
		public void WalkAlong(List<Vector2I> path)
		{
			if (path == null || path.Count == 0)
			{
				EmitSignal(SignalName.UnitMoveFinished);
				return;
			}

			_waypoints.Clear();

			for (int i = 1; i < path.Count; i++)
			{
				_waypoints.Enqueue(_board.TileToWorld(path[i]));
			}

			if (_waypoints.Count == 0)
			{
				EmitSignal(SignalName.UnitMoveFinished);
				return;
			}

			_targetPosition = _waypoints.Dequeue();

			// Triggers the move animation & turns on processing
			IsWalking = true;
		}
	}
}
