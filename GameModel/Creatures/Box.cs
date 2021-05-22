using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Box : ICreature
    {
        public MovementCondition MovementCondition { get; }
        public Direction Direction { get; }

        public Box()
        {
            MovementCondition = MovementCondition.Default;
            Direction = Direction.NoMovement;
        }
    }
}