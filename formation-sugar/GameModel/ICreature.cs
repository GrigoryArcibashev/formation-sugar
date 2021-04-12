using System.Drawing;

namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        Point Location { get; set; }
        int Health { get; }
        void ChangeHealthBy(int deltaHealth);
    }
}