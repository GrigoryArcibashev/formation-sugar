using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Chest : ICreatureWithHealth
    {
        public MovementConditions MovementCondition { get; private set; }
        public Direction Direction { get; }
        public int Health { get; private set; }
        public int Score { get; }

        public Chest(int health)
        {
            MovementCondition = MovementConditions.Default;
            Direction = Direction.NoMovement;
            Health = health;
            Score = 10;
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