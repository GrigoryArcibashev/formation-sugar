using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Box : ICreature
    {
        public MovementConditions MovementCondition { get; }
        public Direction Direction { get; }

        public Box()
        {
            MovementCondition = MovementConditions.Default;
            Direction = Direction.NoMovement;
        }
    }
}