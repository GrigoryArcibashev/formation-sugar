namespace Model.Creatures.CreatureInterfaces
{
    public interface ICreature
    {
        public MovementCondition MovementCondition { get; }
        public Direction Direction { get; }
    }
}