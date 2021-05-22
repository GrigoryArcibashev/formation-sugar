namespace Model.Creatures.CreatureInterfaces
{
    public interface IMovingCreature : ICreature
    {
        public void ChangeMovementConditionAndDirectionTo(MovementCondition movementConditionTo, Direction directionTo);
    }
}