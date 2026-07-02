using System.Collections.Generic;
using System.Linq;
using TEP.Game.Core;

namespace TEP.Game.Combat
{
    // Finds the path between 2 points among walkable tiles using the A* algorithm.
    public partial class PathFinder : RefCounted
    {
        private CombatGrid _grid;
        private AStar2D _astar = new AStar2D();

        // Initializes the AStar2D object upon creation.
        public PathFinder(CombatGrid grid, IEnumerable<Vector2I> walkableTiles)
        {
            _grid = grid;

            // Caches a mapping between tile coordinates & their unique index.
            Dictionary<Vector2I, int> tileMappings = [];
            foreach (var tile in walkableTiles)
            {
                // For each tile, a key-value pair of tile coordinates: index is defined.
                tileMappings[tile] = _grid.AsIndex(tile);
            }

            // All tiles are added to the AStar2D instance & connected to create the pathfinding graph.
            AddAndConnectPoints(tileMappings);
        }

        // Returns the path found between 'start' & 'end' as an array of Vector2I coordinates.
        public List<Vector2I> CalculatePointPath(Vector2I start, Vector2I end)
        {
            int startIndex = _grid.AsIndex(start);
            int endIndex = _grid.AsIndex(end);

            // Ensures the A* graph has both points defined. If not, returns an empty Vector2I array.
            if (_astar.HasPoint(startIndex) && _astar.HasPoint(endIndex))
            {
                return _astar.GetPointPath(startIndex, endIndex).Select(p => (Vector2I)p).ToList();
            }

            return [];
        }

        // Adds & connects the walkable tiles to the AStar2D object.
        private void AddAndConnectPoints(Dictionary<Vector2I, int> tileMappings)
        {
            /* First, all points are registered in the A* graph. Each tile's unique index & corresponding
               Vector2I coordinates are passed into the AStar2D.AddPoint() function. */
            foreach (var kvp in tileMappings)
            {
                Vector2I tile = kvp.Key;
                int index = kvp.Value;

                _astar.AddPoint(index, tile);
            }

            // Points are looped over again & connected with all their neighbors.
            foreach (var kvp in tileMappings)
            {
                Vector2I tile = kvp.Key;
                int index = kvp.Value;

                foreach (var neighborIndex in FindNeighborIndices(tile, tileMappings))
                {
                    _astar.ConnectPoints(index, neighborIndex);
                }
            }
        }

        // Returns an array of the tile's connectable neighbors.
        private List<int> FindNeighborIndices(Vector2I tile, Dictionary<Vector2I, int> tileMappings)
        {
            List<int> result = [];

            // Try to move 1 tile in every possible direction & ensure that the tile is walkable & not already connected.
            foreach (Vector2I direction in GridDirections.Cardinal)
            {
                Vector2I neighbor = tile + direction;

                // Ensures the neighboring tile is part of the walkable tiles.
                if (!tileMappings.ContainsKey(neighbor))
                {
                    continue;
                }

                int neighborIndex = tileMappings[neighbor];
                int tileIndex = tileMappings[tile];

                if (!_astar.ArePointsConnected(tileIndex, neighborIndex))
                {
                    result.Add(neighborIndex);
                }
            }

            return result;
        }
    }
}
