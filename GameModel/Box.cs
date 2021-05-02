using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Point Location { get; set; }
        public Size Size { get; }
        public MovementConditions MovementCondition { get; private set; }

        public Box(Point location, Size size)
        {
            Location = location;
            Size = size;
            MovementCondition = MovementConditions.Default;
        }

        public string GetTypeAsString()
        {
            return "Box";
        }
    }
}