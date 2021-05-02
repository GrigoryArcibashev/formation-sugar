using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Size Size { get; }
        public MovementConditions MovementCondition { get; private set; }

        public Box()
        {
            MovementCondition = MovementConditions.Default;
        }

        public string GetTypeAsString()
        {
            return "Box";
        }
    }
}