using System;

namespace Model.Creatures
{
    public class Enemy : IMovingCreature, IEnemy, ICreatureWithHealth
    {
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; private set; }
        public int Health { get; private set; }

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo)
        {
            MovementCondition = movementConditionTo;
            Direction = directionTo;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
            if (Health == 0)
            {
                MovementCondition = MovementConditions.Dying;
            }
        }
    }
}