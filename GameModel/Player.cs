using System;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Player : IMovingCreature
    {
        private readonly double initialVelocity;
        
        public double Velocity { get; set; }
        public int Health { get; private set; }
        public Point Location { get; set; }
        public MovementConditions MovementCondition { get; set; }
        public Direction Direction { get; set; }

        public Player(Point initialLocation, int initialHealth = 0, double initialVelocity = 0.0)
        {
            Location = initialLocation;
            Health = initialHealth;
            Velocity = initialVelocity;
            this.initialVelocity = initialVelocity;
            MovementCondition = MovementConditions.StandingRight;
            Direction = Direction.Right;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
        }

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementCondition, Direction direction)
        {
            MovementCondition = movementCondition;
            Direction = direction;
        }

        public bool IsPlayerJumping()
        {
            return MovementCondition == MovementConditions.JumpingRight ||
                   MovementCondition == MovementConditions.JumpingLeft;
        }
        
        public bool IsPlayerFalling()
        {
            return MovementCondition == MovementConditions.FallingRight ||
                   MovementCondition == MovementConditions.FallingLeft;
        }

        public void RecoverVelocity()
        {
            Velocity = initialVelocity;
        }
        
        public string GetTypeAsString()
        {
            return "Player";
        }
    }
}