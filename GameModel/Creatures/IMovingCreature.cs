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

        [ChangeCondition]
        [ChangeConditionThatAffectsMovement]
        void ChangeConditionToJumping();

        [ChangeCondition]
        [ChangeConditionThatAffectsMovement]
        void ChangeConditionToFalling();

        [ChangeCondition]
        [ChangeConditionThatAffectsMovement]
        void ChangeConditionToFallingDown();
        
        [ChangeCondition]
        [ChangeConditionThatAffectsMovement]
        void ChangeConditionToRun(Direction directionToChange);
        
        [ChangeCondition]
        void ChangeConditionToStanding();

        [ChangeCondition]
        void ChangeConditionToSitting();
        
        [ChangeCondition]
        void ChangeConditionToAttacking();

        [ChangeCondition]
        void ChangeConditionToDie();
    }
}