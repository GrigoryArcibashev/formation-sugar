namespace Model.Creatures
{
    public interface IMovingCreature : ICreature
    {
        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo);
    }
}