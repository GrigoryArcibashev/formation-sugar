namespace Model.Creatures
{
    public class Enemy : IMovingCreature
    {
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; private set; }

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo)
        {
            MovementCondition = movementConditionTo;
            Direction = directionTo;
        }
    }
}