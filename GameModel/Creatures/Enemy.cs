using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Enemy : IEnemy, IAttackingCreature
    {
        public int ScoreForKilling { get; }
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; private set; }
        public int DamageValue { get; }
        public int Health { get; private set; }

        public Enemy(int damageValue, int initialHealth, int scoreForKilling)
        {
            DamageValue = damageValue;
            Health = initialHealth;
            MovementCondition = MovementConditions.Standing;
            Direction = Direction.Right;
            ScoreForKilling = scoreForKilling;
        }

        public void ChangeMovementConditionAndDirectionTo(MovementConditions movementConditionTo, Direction directionTo)
        {
            MovementCondition = movementConditionTo;
            Direction = directionTo;
        }

        public bool IsDead()
        {
            return MovementCondition is MovementConditions.Dying;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health - deltaHealth);
            
            if (Health == 0)
            {
                MovementCondition = MovementConditions.Dying;
            }
        }
    }
}