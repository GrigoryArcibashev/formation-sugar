using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public MovementConditions MovementCondition { get; set; }

        public Box(Point location, Size size)
        {
            Location = location;
            Size = size;
        }

        public string GetTypeAsString()
        {
            return "Box";
        }
    }
}