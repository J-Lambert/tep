using System;
using Godot;

public partial class Player : Node2D
{
	[Export] public int TileSize = 16;
	[Export] public float moveDelay = 0.2f;

	public Vector2I GridPosition = new Vector2I(1, 1);

	private float moveTimer = 0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    private Vector2I GetInputDirection()
	{
		int x = 0;
		int y = 0;

		if (Input.IsActionPressed("move_left"))
		{
			x -= 1;
		}
		if (Input.IsActionPressed("move_right"))
		{
			x += 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			y -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			y += 1;
		}

		if (x == 0 && y == 0)
		{
			return Vector2I.Zero;
		}

		if (x != 0 && y != 0)
		{
			return new Vector2I(0, y);
		}

		return new Vector2I(x, y);
	}

	private void TryMove(Vector2I direction)
	{
		Vector2I targetPos = GridPosition + direction;

		GridPosition = targetPos;
	}

	private void UpdateWorldPosition()
	{
		Position = new Vector2(GridPosition.X * TileSize, GridPosition.Y * TileSize);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		moveTimer -= (float)delta;

		if (moveTimer > 0)
		{
			return;
		}

		Vector2I dir = GetInputDirection();

		if (dir != Vector2I.Zero)
		{
			moveTimer = moveDelay;
		}
	}
}
