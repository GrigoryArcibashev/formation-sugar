namespace Model.Creatures
{
    public interface IJumpingCreature : IMovingCreature
    {
        int Velocity { get; }
        void RecoverVelocity();
        void ResetVelocityToZero();
        void IncreaseVelocity();
        void ReduceVelocity();
        bool IsJumping();
        bool IsFalling();
    }
}