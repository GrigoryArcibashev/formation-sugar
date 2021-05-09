namespace Model.Creatures
{
    public interface IJumpingCreature : IMovingCreature
    {
        public int Velocity { get; }
        public void RecoverVelocity();
        public void ResetVelocityToZero();
        public void IncreaseVelocity();
        public void ReduceVelocity();
        public bool IsJumping();
        public bool IsFalling();
    }
}