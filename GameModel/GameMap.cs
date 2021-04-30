using System;
using System.Collections.Generic;
using System.IO;

namespace formation_sugar.GameModel
{
    public class GameMap
    {
        private readonly List<string> levels;
        
        public List<ICreature> CreaturesToDraw { get; private set; }
        public ICreature[,] Map { get; private set; }
        public Player Player { get; private set; }

        public GameMap(int levelNumber)
        {
            CreaturesToDraw = new List<ICreature>();
            levels = new List<string>();
            AddLevels();
            CreateMap(levelNumber);
        }

        private void CreateMap(int levelNumber)
        {
            var mapInfo = MapCreator.CreateMap(File.ReadAllLines(levels[levelNumber - 1]));
            CreaturesToDraw = mapInfo.CreaturesToDraw;
            Player = mapInfo.Player;
            Map = mapInfo.Map;
        }

        private void AddLevels()
        {
            var directoryWithLevels = new DirectoryInfo(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ??
                throw new InvalidOperationException(), "levels"));
            
            foreach (var levelName in directoryWithLevels.EnumerateFiles())
            {
                levels.Add(levelName.FullName);
            }
        }
    }
}