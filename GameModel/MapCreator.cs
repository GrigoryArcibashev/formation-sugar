using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;

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
            var createdMap = ParseStringToLevel(File.ReadAllLines(levels[numberOfCurrentLevel]));
            numberOfCurrentLevel = (numberOfCurrentLevel + 1) % levels.Length;
            return createdMap;
        }

        public static void ResetLevel()
        {
            numberOfCurrentLevel = numberOfCurrentLevel > 0
                ? numberOfCurrentLevel -= 1
                : numberOfCurrentLevel = levels.Length - 1;
        }

        public static void GoToLevel(string levelName)
        {
            var index = Array.IndexOf(levels, Path.Combine(fullPathToLevels.FullName, levelName));
            numberOfCurrentLevel = index != -1
                ? index
                : throw new FileNotFoundException($"File '{levelName}' not found");
        }

        private static MapInfo ParseStringToLevel(IReadOnlyList<string> level)
        {
            map = new ICreature[level.First().Length, level.Count];
            listOfCreatures = new List<ICreature>();
            Player player = default;
            Finish finish = default;

            for (var y = 0; y < level.Count; y++)
            {
                for (var x = 0; x < level.First().Length; x++)
                {
                    switch (level[y][x].ToString())
                    {
                        case "P":
                            player = new Player(100, 100, 2);
                            AddCreatureOnMapAndListOfCreatures(player, new Point(x, y));
                            break;

                        case "E":
                            AddCreatureOnMapAndListOfCreatures(new Enemy(5, 100, 10), new Point(x, y));
                            break;
                        
                        case "X":
                            AddCreatureOnMapAndListOfCreatures(new BossEnemy(20, 300, 500), new Point(x, y));
                            break;

                        case "B":
                            AddCreatureOnMapAndListOfCreatures(new Box(), new Point(x, y));
                            break;

                        case "C":
                            AddCreatureOnMapAndListOfCreatures(new Chest(1), new Point(x, y));
                            break;

                        case "F":
                            finish = new Finish();
                            AddCreatureOnMapAndListOfCreatures(finish, new Point(x, y));
                            break;
                    }
                }
            }

            if (player == default)
                throw new Exception("You forgot to add a player on the level");
            
            if (finish == default)
                throw new Exception("You forgot to add a finish on the level");

            return new MapInfo(map, listOfCreatures, player, finish);
        }

        private static string[] GetLevelsFrom(string localPathToLevelsRelativeToSolutionDirectory)
        {
            fullPathToLevels = new DirectoryInfo(
                Path.Combine(
                    new DirectoryInfo(
                        Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ??
                    throw new FileNotFoundException(),
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