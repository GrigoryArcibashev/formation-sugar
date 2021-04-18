using System;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Player : ICreature
    {
        public Point Location { get; set; }
        public int Health { get; private set; }
        
        public Direction Direction { get; set; }
        public double Velocity { get; set; }
        public MovementConditions MovementsCondition { get; set; }

        public Player(Point initialLocation, int initialHealth = 0, double velocity = 0.0)
        {
            Location = initialLocation;
            Health = initialHealth;
            Velocity = velocity;
            MovementsCondition = MovementConditions.StandingRight;
            Direction = Direction.Right;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
        }

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementCondition, Direction direction)
        {
            MovementsCondition = movementCondition;
            Direction = direction;
        }
    }
}