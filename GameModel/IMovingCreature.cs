namespace formation_sugar.GameModel
{
    public interface IMovingCreature : ICreature
    {
        int Velocity { get; set; }
        Direction Direction { get; set; }
        void RecoverVelocity();
        bool IsPlayerJumping();
        bool IsPlayerFalling();
    }
}