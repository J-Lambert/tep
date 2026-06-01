
using System.ComponentModel;

namespace TEP.Game.Combat
{
	public partial class CombatBoard : Node2D
	{
		[Export] public PackedScene TileScene;
		[Export] public int BoardWidth = 9;
		[Export] public int BoardHeight = 9;
		[Export] public int TileSize = 16;
		[Export] public Texture2D MainTileTexture;
		[Export] public Texture2D AltTileTexture;

		private CombatTile[,] _tiles;
		private int _halfTileSize = 0;

		public override void _Ready()
		{
			_halfTileSize = TileSize / 2;

			GenerateBoard();
		}

		private void GenerateBoard()
		{
			_tiles = new CombatTile[BoardWidth, BoardHeight];

			for (int y = 0; y < BoardHeight; y++)
			{
				for (int x = 0; x < BoardWidth; x++)
				{
					CombatTile tile = TileScene.Instantiate<CombatTile>();

					AddChild(tile);
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

