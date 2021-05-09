using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Model.Creatures;

namespace Model
{
    public static class MapCreator
    {
        private static DirectoryInfo fullPathToLevels;
        private static string[] levels;
        private static int numberOfCurrentLevel;
        private static ICreature[,] map;
        private static List<ICreature> listOfCreatures;

        static MapCreator()
        {
            LoadLevels("Levels");
        }

        public static void LoadLevels(string localPathToLevelsRelativeToSolutionDirectory)
        {
            levels = GetLevelsFrom(localPathToLevelsRelativeToSolutionDirectory);
            if (levels.Length == 0)
                throw new FileNotFoundException(
                    $"There are no levels in the directory '{localPathToLevelsRelativeToSolutionDirectory}'");
            numberOfCurrentLevel = 0;
        }

        public static MapInfo GetNextMap()
        {
            var createdMap = CreateMap(File.ReadAllLines(levels[numberOfCurrentLevel]));
            numberOfCurrentLevel = (numberOfCurrentLevel + 1) % levels.Length;
            return createdMap;
        }

        public static void GoToLevel(string levelName)
        {
            var index = Array.IndexOf(levels, Path.Combine(fullPathToLevels.FullName, levelName));
            numberOfCurrentLevel = index != -1
                ? index
                : throw new FileNotFoundException($"File '{levelName}' not found");
        }

        private static MapInfo CreateMap(IReadOnlyList<string> level)
        {
            var levelSize = level[0]
                .Split()
                .Select(int.Parse)
                .ToArray();

            map = new ICreature[levelSize[0], levelSize[1]];
            listOfCreatures = new List<ICreature>();
            Player player = default;

            foreach (var line in level.Skip(1))
            {
                var parts = line.Split();
                var coordinates = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
                switch (parts[0])
                {
                    case "P":
                        player = new Player(50, 100, 2);
                        AddCreatureOnMapAndListOfCreatures(player, coordinates);
                        break;
                    
                    case "E":
                        AddCreatureOnMapAndListOfCreatures(new Enemy(25, 100), coordinates);
                        break;

                    case "B":
                        AddCreatureOnMapAndListOfCreatures(new Box(), coordinates);
                        break;
                }
            }

            if (player == default)
                throw new Exception("You forgot to add a player on the level");
            return new MapInfo(map, player, listOfCreatures);
        }


        private static string[] GetLevelsFrom(string localPathToLevelsRelativeToSolutionDirectory)
        {
            fullPathToLevels = new DirectoryInfo(
                Path.Combine(
                    new DirectoryInfo(
                        Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ?? throw new FileNotFoundException(),
                    localPathToLevelsRelativeToSolutionDirectory));
            return fullPathToLevels
                .EnumerateFiles()
                .Select(file => file.FullName)
                .ToArray();
        }

        private static void AddCreatureOnMapAndListOfCreatures(ICreature creature, Point coordinates)
        {
            listOfCreatures.Add(creature);
            map[coordinates.X, coordinates.Y] = creature;
        }
    }
}