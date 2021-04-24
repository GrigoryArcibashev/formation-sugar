using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Point Location { get; set; }
        public MovementConditions MovementCondition { get; set; }
        public Direction Direction { get; set; }
        public double Velocity { get; set; }
        public int Health { get; }

        public Box(Point location, int health)
        {
            Location = location;
            Health = health;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            throw new System.NotImplementedException();
        }
    }
}