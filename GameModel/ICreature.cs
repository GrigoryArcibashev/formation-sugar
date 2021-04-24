using System.Drawing;

namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        Point Location { get; set; }
        MovementConditions MovementCondition { get; set; }
        string GetTypeAsString();
    }
}