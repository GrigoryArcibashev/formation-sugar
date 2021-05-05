using System;

namespace Model.Creatures
{
    public class Player : IMovingCreature
    {
        private readonly int initialVelocity;
        private Direction direction;
        public int Velocity { get; private set; }
        public int Health { get; private set; }
        public MovementConditions MovementCondition { get; private set; }


        public Player(int initialHealth = 0, int initialVelocity = 0)
        {
            Health = initialHealth;
            Velocity = initialVelocity;
            this.initialVelocity = initialVelocity;
            MovementCondition = MovementConditions.StandingRight;
            direction = Direction.Right;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
        }


        public bool IsJumping()
        {
            return MovementCondition == MovementConditions.JumpingRight ||
                   MovementCondition == MovementConditions.JumpingLeft;
        }

        public bool IsFalling()
        {
            return MovementCondition == MovementConditions.FallingRight ||
                   MovementCondition == MovementConditions.FallingLeft ||
                   MovementCondition == MovementConditions.FallingDown;
        }

        public void ChangeConditionToStanding()
        {
            MovementCondition = direction is Direction.Right
                ? MovementConditions.StandingRight
                : MovementConditions.StandingLeft;
        }

        public void ChangeConditionToSitting()
        {
            MovementCondition = direction is Direction.Right
                ? MovementConditions.SittingRight
                : MovementConditions.SittingLeft;
        }

        public void ChangeConditionToJumping()
        {
            MovementCondition = direction is Direction.Right
                ? MovementConditions.JumpingRight
                : MovementConditions.JumpingLeft;
        }

        public void ChangeConditionToFalling()
        {
            MovementCondition = direction is Direction.Right
                ? MovementCondition = MovementConditions.FallingRight
                : MovementCondition = MovementConditions.FallingLeft;
        }

        
        public void ChangeConditionToFallingDown()
        {
            MovementCondition = MovementConditions.FallingDown;
        }

        public void ChangeConditionToAttacking()
        {
            MovementCondition = direction is Direction.Right
                ? MovementCondition = MovementConditions.AttackingRight
                : MovementCondition = MovementConditions.AttackingLeft;
        }

        public void ChangeConditionToDie()
        {
            MovementCondition = direction is Direction.Right
                ? MovementCondition = MovementConditions.DieRight
                : MovementCondition = MovementConditions.DieLeft;
        }

        public void ChangeConditionToRun(Direction directionToChange)
        {
            MovementCondition = directionToChange is Direction.Right
                ? MovementCondition = MovementConditions.RunningRight
                : MovementCondition = MovementConditions.RunningLeft;
            this.direction = directionToChange;
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