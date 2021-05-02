﻿using System;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Player : IMovingCreature
    {
        private readonly int initialVelocity;
        private Direction direction;

        public Point Location { get; set; }
        public Size Size { get; set; }
        public int Velocity { get; set; }
        public int Health { get; private set; }
        public MovementConditions MovementCondition { get; set; }


        public Player(Point initialLocation, Size size, int initialHealth = 0, int initialVelocity = 0)
        {
            Location = initialLocation;
            Size = size;
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

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementCondition, Direction direction)
        {
            MovementCondition = movementCondition;
            this.direction = direction;
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
            MovementCondition = direction == Direction.Right
                ? MovementCondition = MovementConditions.FallingRight
                : MovementCondition = MovementConditions.FallingLeft;
        }

        public void ChangeConditionToFallingDown()
        {
            MovementCondition = MovementConditions.FallingDown;
        }

        public void ChangeConditionToRun(Direction direction)
        {
            MovementCondition = direction == Direction.Right
                ? MovementCondition = MovementConditions.RunningRight
                : MovementCondition = MovementConditions.RunningLeft;
            this.direction = direction;
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