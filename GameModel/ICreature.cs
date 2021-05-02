using System.Drawing;

namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        Point Location { get; set; }
        Size Size { get; }
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}