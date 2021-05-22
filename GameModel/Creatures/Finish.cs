using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Finish : ICreatureWithHealth
    {
        public MovementCondition MovementCondition { get; private set; }
        public Direction Direction { get; }
        public int Health { get; private set; }

        public Finish()
        {
            MovementCondition = MovementCondition.Default;
            Direction = Direction.NoMovement;
            Health = 1;
        }

        public bool IsDead()
        {
            return MovementCondition is MovementCondition.Dying;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health - deltaHealth);

            if (Health == 0)
            {
                MovementCondition = MovementCondition.Dying;
            }
        }
    }
}