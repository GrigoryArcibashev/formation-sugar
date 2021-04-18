using System.Drawing;

namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        Point Location { get; set; }
        MovementConditions MovementsCondition { get; set; }
        Direction Direction { get; set; }
        double Velocity { get; set; }
        int Health { get; }

        void ChangeHealthBy(int deltaHealth);
    }
}