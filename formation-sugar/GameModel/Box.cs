using System.Drawing;
using System.IO;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
        public Point Location { get; set; }
        public double Velocity { get; set; }
        public int Health { get; }
        public Sprite Sprite { get; }

        public Box(Point location, int health)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent;
            Sprite = new Sprite(
                1,
                new Size(24, 24),
                new Bitmap(Path.Combine(currentDirectory?.FullName!, @"Sprites\grass.png")));
            Location = location;
            Health = health;
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            throw new System.NotImplementedException();
        }
    }
}