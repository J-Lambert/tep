namespace TEP.Game.Combat
{
    public partial class UnitStats : GodotObject
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Damage { get; set; }
        public int MoveRange { get; set; }
        public int MoveSpeed { get; set; }
    }
}
