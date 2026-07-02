using System.Collections.Generic;

namespace TEP.Game.Combat.UI
{
	// Draws an overlay over an array of tiles.
	public partial class UnitOverlay : TileMapLayer
	{
		// Allows inspector selection of specific tileset & tile for overlay.
		[Export] public int SourceId = 0;
		[Export] public Vector2I AtlasCoords = Vector2I.Zero;

		public new void Draw(List<Vector2I> tiles)
		{
			Clear();

			foreach (Vector2I tile in tiles)
			{
				SetCell(tile, SourceId, AtlasCoords);
			}
		}
	}
}
