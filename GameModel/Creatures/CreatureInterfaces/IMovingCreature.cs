namespace Model.Creatures.CreatureInterfaces
{
    public interface IMovingCreature : ICreature
    {
        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo);
    }
}