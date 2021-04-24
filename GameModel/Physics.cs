using System;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Physics
    {
        private readonly double g;

        public Physics(double g)
        {
            this.g = g;
        }

        public void MoveCreatureByY(ICreature creature, double dt)
        {
            creature.Location = new Point(
                creature.Location.X,
                creature.Location.Y - (int) (creature.Velocity * Math.Abs(creature.Velocity)));
            creature.Velocity += g * dt;
        }
    }
}