namespace TEP.Game.Combat
{
	/* Represents a grid of tiles with its size, the size of each tile in pixels, & some helper functions to calculate
	   & convert coordinates. Meant to be shared between game objects that need access to those values.*/
	public partial class CombatBoard : Node2D
	{
		[Export] public PackedScene TileScene;

		// Chessboard width & height in terms of tiles.
		[Export(PropertyHint.Range, "1,10000")] public int BoardWidth = 9;
		[Export(PropertyHint.Range, "1,10000")] public int BoardHeight = 9;

		// Tile size in pixels.
		[Export(PropertyHint.Range, "1,1024")] public int TileSize = 16;

		[Export] public Texture2D MainTileTexture;
		[Export] public Texture2D AltTileTexture;

		// Margin around board in terms of a float percentage (0.1 = 10%)
		[Export(PropertyHint.Range, "-3.0,3.0,0.01")] public float BoardPaddingX = 0.1f;
		[Export(PropertyHint.Range, "-3.0,3.0,0.01")] public float BoardPaddingY = 0.1f;

		private Node2D _tileContainer;
		private Node2D _unitContainer;
		private Camera2D _camera;

		private CombatTile[,] _tiles;
		private int _halfTileSize = 0;

		public override void _Ready()
		{
			ValidateBoardSettings();

			// Prevents calculating this for every tile created.
			_halfTileSize = TileSize / 2;

			_tileContainer = GetNode<Node2D>("TileContainer");
			_unitContainer = GetNode<Node2D>("UnitContainer");
			_camera = GetNode<Camera2D>("CombatCamera");

			GenerateBoard();
			SetupCamera();
		}

		public CombatTile GetTile(Vector2I position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= BoardWidth || position.Y >= BoardHeight)
			{
				return null;
			}

			return _tiles[position.X, position.Y];
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

		private void SetupCamera()
		{
			_camera.Enabled = true;

			float boardPixelWidth = BoardWidth * TileSize;
			float boardPixelHeight = BoardHeight * TileSize;

			Vector2 viewportSize = GetViewportRect().Size;

			float zoomX = viewportSize.X / boardPixelWidth;
			float zoomY = viewportSize.Y / boardPixelHeight;

			// Takes the smaller dimension in cases of a rectangular board.
			float zoom = Mathf.Min(zoomX, zoomY);

			// Center camera on combat board.
			_camera.Position = new Vector2(boardPixelWidth / 2.0f, boardPixelHeight / 2.0f);

			_camera.Zoom = new Vector2(zoom * (1 - BoardPaddingX), zoom * (1 - BoardPaddingY));
		}

		// Prevents invalid values being passed into various board functions.
		private void ValidateBoardSettings()
		{
				BoardWidth = Mathf.Max(BoardWidth, 1);
				BoardHeight = Mathf.Max(BoardHeight, 1);
				TileSize = Mathf.Max(TileSize, 1);
		}
	}
}

