namespace TEP.Game
{
	public partial class Tile : Node2D
	{
		[Export] public Sprite2D TileSprite;

		public void SetTexture(Texture2D texture)
		{
			TileSprite.Texture = texture;
		}
	}
}