using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Point Location { get; set; }
        public MovementConditions MovementCondition { get; set; }
       
        public Box(Point location)
        {
            Location = location;
        }
        
        public string GetTypeAsString()
        {
            return "Box";
        }
    }
}