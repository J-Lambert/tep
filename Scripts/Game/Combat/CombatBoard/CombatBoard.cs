
using System.ComponentModel;

namespace TEP.Game.Combat
{
	/* Represents a grid of tiles with its size, the size of each tile in pixels, & some helper functions to calculate
	   & convert coordinates. Meant to be shared between game objects that need access to those values.*/
	public partial class CombatBoard : Node2D
	{
		[Export] public PackedScene TileScene;

		// Chessboard width & height in terms of tiles.
		[Export] public int BoardWidth = 9;
		[Export] public int BoardHeight = 9;

		// Tile size in pixels.
		[Export] public int TileSize = 16;
		[Export] public Texture2D MainTileTexture;
		[Export] public Texture2D AltTileTexture;

		private Node2D _tileContainer;
		private Node2D _unitContainer;

		private CombatTile[,] _tiles;
		private int _halfTileSize = 0;

		public override void _Ready()
		{
			// Prevents calculating this for every tile created.
			_halfTileSize = TileSize / 2;

			_tileContainer = GetNode<Node2D>("TileContainer");
			_unitContainer = GetNode<Node2D>("UnitContainer");

			GenerateBoard();
		}

		private void GenerateBoard()
		{
			// Initializes 2D array of tiles with a set width & height.
			_tiles = new CombatTile[BoardWidth, BoardHeight];

			for (int y = 0; y < BoardHeight; y++)
			{
				for (int x = 0; x < BoardWidth; x++)
				{
					CombatTile tile = TileScene.Instantiate<CombatTile>();

					_tileContainer.AddChild(tile);
					tile.Position = new Vector2I((x * TileSize) + _halfTileSize, (y * TileSize) + _halfTileSize);

					bool mainTile = (x + y) % 2 == 0;

					tile.Initialize(new Vector2I(x, y), mainTile ? MainTileTexture : AltTileTexture);

					_tiles[x, y] = tile;
				}
			}
		}

		public CombatTile GetTile(Vector2I position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= BoardWidth || position.Y >= BoardHeight)
			{
				return null;
			}

			return _tiles[position.X, position.Y];
		}
	}
}

