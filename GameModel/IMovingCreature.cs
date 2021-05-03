namespace formation_sugar.GameModel
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

        [ChangeCondition]
        void ChangeConditionToStanding();

        [ChangeCondition]
        void ChangeConditionToSitting();

        [ChangeCondition]
        void ChangeConditionToJumping();

        [ChangeCondition]
        void ChangeConditionToFalling();

        [ChangeCondition]
        void ChangeConditionToFallingDown();

        [ChangeCondition]
        void ChangeConditionToAttacking();

        [ChangeCondition]
        void ChangeConditionToDie();

        [ChangeCondition]
        void ChangeConditionToRun(Direction direction);
    }
}