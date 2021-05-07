using System;

namespace Model.Creatures
{
    public class Player : IMovingCreature
    {
        private readonly int initialVelocity;
        public int Velocity { get; private set; }
        public int Health { get; private set; }
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; private set; }

        public Player(int initialHealth = 0, int initialVelocity = 0)
        {
            Health = initialHealth;
            Velocity = initialVelocity;
            this.initialVelocity = initialVelocity;
            MovementCondition = MovementConditions.Standing;
            Direction = Direction.Right;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
            if (Health == 0)
            {
                MovementCondition = MovementConditions.Dying;
            }
        }


        public bool IsJumping()
        {
            return MovementCondition == MovementConditions.Jumping;
        }

        public bool IsFalling()
        {
            return MovementCondition == MovementConditions.Falling;
        }
        
        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo)
        {
            MovementCondition = movementConditionTo;
            Direction = directionTo;
        }


        public void RecoverVelocity()
        {
            Velocity = initialVelocity;
        }

        public void ResetVelocityToZero()
        {
            Velocity = 0;
        }

        public void IncreaseVelocity()
        {
            Velocity++;
        }

        public void ReduceVelocity()
        {
            Velocity--;
        }

        public string GetTypeAsString()
        {
            return "Player";
        }
    }
}