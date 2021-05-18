namespace Model.Creatures.CreatureInterfaces
{
    public interface ICreature
    {
        public MovementConditions MovementCondition { get; }
        public Direction Direction { get; }
    }
}