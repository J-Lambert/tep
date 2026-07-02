namespace TEP.Game.Combat
{
    /* Represents a grid with its size, the size of each tile in pixels, & some helper functions to calculate &
   convert coordinates. It's meant to be shared between game objects that need access to those values. */
    [GlobalClass]
    public partial class CombatGrid : Resource
    {
        // The grid's size in rows & columns.
        [Export(PropertyHint.Range, "1,500")] public Vector2I GridSize = new(9, 9);

        // The size of a tile in pixels.
        [Export(PropertyHint.Range, "1,1024")] public Vector2I TileSize = new(16, 16);

        // Used to calculate the center of a grid tile in pixels.
        public Vector2 HalfTileSize => TileSize / 2;

        // Converts 2D tile coordinates into a 1D array index.
        public int AsIndex(Vector2I tile)
        {
            return tile.X + GridSize.X * tile.Y;
        }

        // Returns the coordinates of the tile on the grid given a position on the map.
        public Vector2I CalculateGridCoordinates(Vector2 mapPos)
        {
            return (Vector2I)(mapPos / TileSize).Floor();
        }

        // Returns the position of a tile's center in pixels.
        public Vector2 CalculateMapPosition(Vector2 gridPos)
        {
            return gridPos * TileSize + HalfTileSize;
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
