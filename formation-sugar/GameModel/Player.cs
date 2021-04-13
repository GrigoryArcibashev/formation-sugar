using System;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Player : ICreature
    {
        public Point Location { get; set; }
        public int Health { get; private set; }
        public double Velocity { get; set; }

        public Player(Point initialLocation, int initialHealth = 0, double velocity = 0.0)
        {
            Location = initialLocation;
            Health = initialHealth;
            Velocity = velocity;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
        }
    }
}