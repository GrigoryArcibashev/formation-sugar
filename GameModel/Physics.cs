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

        public void MoveCreatureByY(IMovingCreature movingCreature, double dt)
        {
            movingCreature.Location = new Point(
                movingCreature.Location.X,
                movingCreature.Location.Y - (int) (movingCreature.Velocity * Math.Abs(movingCreature.Velocity)));
            //movingCreature.Velocity += g * dt;
        }
    }
}