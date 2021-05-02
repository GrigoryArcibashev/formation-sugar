using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace formation_sugar.GameModel
{
    public static class MapCreator
    {
        private static ICreature[,] map;
        private static List<ICreature> listOfCreatures;

        public static MapInfo CreateMap(string[] level)
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
                        player = new Player(coordinates, new Size(3, 4),initialVelocity:8);
                        AddCreatureOnMapAndListOfCreatures(player, coordinates);
                        break;

                    case "G":
                        var box = new Box(coordinates, new Size(1, 1));
                        AddCreatureOnMapAndListOfCreatures(box, coordinates);
                        break;
                }
            }

            if (player == default)
                throw new Exception("You forgot to add player on the level");
            return new MapInfo(map, player, listOfCreatures);
        }

        private static void AddCreatureOnMapAndListOfCreatures(ICreature creature, Point coordinates)
        {
            listOfCreatures.Add(creature);
            map[coordinates.X, coordinates.Y] = creature;
        }
    }
}