namespace Model.Creatures
{
    public interface ICreature
    {
        public MovementConditions MovementCondition { get; }
        public Direction Direction { get; }
        public bool IsDead();
    }
}