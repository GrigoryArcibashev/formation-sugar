namespace Model.Creatures
{
    public interface IMovingCreature : ICreature
    {
        void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo);
    }
}