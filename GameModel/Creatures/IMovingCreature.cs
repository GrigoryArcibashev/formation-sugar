namespace Model.Creatures
{
    public interface IMovingCreature : ICreature
    {
        int Velocity { get; }
        void RecoverVelocity();
        void ResetVelocityToZero();
        void IncreaseVelocity();
        void ReduceVelocity();
        bool IsJumping();
        bool IsFalling();
        void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo);
    }
}