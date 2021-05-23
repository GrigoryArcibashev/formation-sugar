using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Player : IJumpingCreature, IAttackingCreature
    {
        private readonly int initialVelocity;
        public int Velocity { get; private set; }
        public int DamageValue { get; }
        public int Health { get; private set; }
        public MovementCondition MovementCondition { get; private set; }
        public Direction Direction { get; private set; }

        public Player(int damageValue, int initialHealth, int initialVelocity)
        {
            DamageValue = damageValue;
            Health = initialHealth;
            Velocity = initialVelocity;
            this.initialVelocity = initialVelocity;
            MovementCondition = MovementCondition.Standing;
            Direction = Direction.Right;
        }

        public void ChangeMovementConditionAndDirectionTo(MovementCondition movementConditionTo, Direction directionTo)
        {
            MovementCondition = movementConditionTo;
            Direction = directionTo;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health - deltaHealth);

            if (Health == 0)
            {
                MovementCondition = MovementCondition.Dying;
            }
        }

        public bool IsDead()
        {
            return MovementCondition is MovementCondition.Dying;
        }

        public bool IsStanding()
        {
            return MovementCondition == MovementCondition.Standing;
        }

        public bool IsRunning()
        {
            return MovementCondition == MovementCondition.Running;
        }

        public bool IsJumping()
        {
            return MovementCondition == MovementCondition.Jumping;
        }

        public bool IsFalling()
        {
            return MovementCondition == MovementCondition.Falling;
        }

        public bool IsAttacking()
        {
            return MovementCondition == MovementCondition.Attacking;
        }

        public bool IsFallingOrJumping()
        {
            return IsFalling() || IsJumping();
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
    }
}