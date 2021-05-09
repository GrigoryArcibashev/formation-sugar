namespace Model.Creatures
{
    public class Box : ICreature
    {
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; }

        public Box()
        {
            MovementCondition = MovementConditions.Default;
            Direction = Direction.NoMovement;
        }
    }
}