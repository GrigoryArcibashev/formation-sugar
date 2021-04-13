using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Image boxImage =
            new Bitmap(@"C:\Users\Win10_Game_OS\Desktop\game\Game\formation-sugar\sprites\grass.png");

        public Box(Point location, int health)
        {
            Location = location;
            Health = health;
        }

        public Point Location { get; set; }
        public double Velocity { get; set; }
        public int Health { get; }
        
        public void ChangeHealthBy(int deltaHealth)
        {
            throw new System.NotImplementedException();
        }
    }
}