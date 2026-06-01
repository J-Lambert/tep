namespace TEP.Game.Combat
{
	public partial class CombatTile : Node2D
	{
		[Export] public Sprite2D TileSprite;

		public Vector2I GridPosition
		{
			get;
			private set;
		}

		public void Initialize(Vector2I gridPosition, Texture2D texture)
		{
			GridPosition = gridPosition;
			TileSprite.Texture = texture;
		}
	}
}