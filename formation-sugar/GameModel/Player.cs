using System;
using System.Drawing;
using System.IO;

namespace formation_sugar.GameModel
{
    enum PlayerMovement
    {
        Standing = 0,
        Sitting = 10,
        Running = 1
    }

    public class Player : ICreature
    {
        private static Image PlayerImage;

        public readonly Sprite Sprite;
        public Point Location { get; set; }
        public int Health { get; private set; }
        public double Velocity { get; set; }

        public int MovementCondition { get; private set; }

        public Player(Point initialLocation, int initialHealth = 0, double velocity = 0.0)
        {
            Location = initialLocation;
            Health = initialHealth;
            Velocity = velocity;
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent;
            PlayerImage = new Bitmap(Path.Combine(currentDirectory?.FullName!, @"Sprites\playerRun.png"));
            Sprite = new Sprite(4, new Size(50, 37), PlayerImage);
        }

        public void ChangeHealthBy(int deltaHealth)
        {
            Health = Math.Max(0, Health + deltaHealth);
        }

        public void ChangeMovementConditionToStanding()
        {
            MovementCondition = (int) PlayerMovement.Standing;
        }

        public void ChangeMovementConditionToRunning()
        {
            MovementCondition = (int) PlayerMovement.Running;
        }

        public void ChangeMovementConditionToSitting()
        {
            MovementCondition = (int) PlayerMovement.Sitting;
        }
    }
}