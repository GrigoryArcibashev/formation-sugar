using System.Drawing;
using System.IO;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public readonly Image BoxImage;
        
        public Box(Point location, int health)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent;
            BoxImage = new Bitmap(Path.Combine(currentDirectory?.FullName!, @"Sprites\grass.png"));
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