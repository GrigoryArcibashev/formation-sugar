using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Chest : ICreatureWithHealth
    {
        private int Health { get; set; }
        
        public MovementCondition MovementCondition { get; private set; }
        public Direction Direction { get; }
        public int Score { get; }

        public Chest(int health)
        {
            MovementCondition = MovementCondition.Default;
            Direction = Direction.NoMovement;
            Health = health;
            Score = 50;
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