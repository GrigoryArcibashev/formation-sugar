﻿namespace Model.Creatures
{
    public class Box : ICreature
    {
        public MovementConditions MovementCondition { get; }
        public Direction Direction { get; }

        public Box()
        {
            MovementCondition = MovementConditions.Default;
            Direction = Direction.NoMovement;
        }
        
        public bool IsDead()
        {
            return MovementCondition is MovementConditions.Dying;
        }
    }
}