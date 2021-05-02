namespace formation_sugar.GameModel
{
    public interface IMovingCreature : ICreature
    {
        int Velocity { get; set; }
        void RecoverVelocity();
        bool IsJumping();
        bool IsFalling();
        void ChangeConditionToStanding();
        void ChangeConditionToSitting();
        void ChangeConditionToJumping();
        void ChangeConditionToFalling();
        void ChangeConditionToFallingDown();
        void ChangeConditionToRun(Direction direction);
    }
}