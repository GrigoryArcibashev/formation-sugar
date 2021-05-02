using System.Drawing;

namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        Size Size { get; }
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}