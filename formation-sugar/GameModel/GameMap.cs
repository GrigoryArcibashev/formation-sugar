using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace formation_sugar.GameModel
{
    public class GameMap
    {
        private readonly List<string> levels;
        public List<ICreature> Map { get; private set; }
        public Player Player { get; private set; }

        public GameMap()
        {
            Map = new List<ICreature>();
            levels = new List<string>();
            var directoryWithLevels = new DirectoryInfo(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ??
                throw new InvalidOperationException(), "levels"));
            
            foreach (var levelName in directoryWithLevels.EnumerateFiles())
            {
                levels.Add(levelName.FullName);
            }
        }

        public void CreateMap(int levelNumber)
        {
            var level = levels[levelNumber - 1];
            var lines = File.ReadAllLines(level);
            foreach (var line in lines)
            {
                var parts = line.Split();
                var coordinates = new Point(int.Parse(parts[1]), int.Parse(parts[2]));

                switch (parts[0])
                {
                    case "P":
                        Player = new Player(coordinates, 100, 8);
                        Map.Add(Player);
                        break;
                    
                    case "G":
                        Map.Add(new Box(coordinates, 100));
                        break;
                }
            }
        }
    }
}