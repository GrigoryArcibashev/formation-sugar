using System;
using Model.Creatures.CreatureInterfaces;

namespace Model.Creatures
{
    public class Finish : ICreatureWithHealth
    { 
        private int Health { get; set; }
        
        public MovementCondition MovementCondition { get; private set; }
        public Direction Direction { get; }
        public Finish()
        {
            MovementCondition = MovementCondition.Default;
            Direction = Direction.NoMovement;
            Health = 1;
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