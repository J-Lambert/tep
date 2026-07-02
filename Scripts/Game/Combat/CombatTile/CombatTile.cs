namespace TEP.Game.Combat
{
	public partial class CombatTile : Node2D
	{
		[Export] public Sprite2D TileSprite;

		public Vector2I BoardPosition
		{
			get;
			private set;
		}

		public void Initialize(Vector2I boardPosition, Texture2D texture)
		{
			BoardPosition = boardPosition;
			TileSprite.Texture = texture;
		}
	}
}