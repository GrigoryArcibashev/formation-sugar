using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace formation_sugar.GameModel
{
    public static class MapCreator
    {
        private const int cellSizeInPixels = 50;

        public static MapInfo CreateMap(string[] level)
        {
            var levelSize = level[0]
                .Split()
                .Select(int.Parse)
                .ToArray();
            var map = new ICreature[levelSize[0], levelSize[1]];
            Player player = default;
            var creatures = new List<ICreature>();

            foreach (var line in level.Skip(1))
            {
                var parts = line.Split();
                var coordinates = new Point(int.Parse(parts[1]), int.Parse(parts[2]));

                switch (parts[0])
                {
                    case "P":
                        player = new Player(coordinates, 100, 2);
                        creatures.Add(player);
                        break;

                    case "G":
                        var box = new Box(new Point(coordinates.X * cellSizeInPixels, coordinates.Y * cellSizeInPixels));
                        creatures.Add(box);
                        map[coordinates.X, coordinates.Y] = box;
                        break;
                }
            }

            if (player == default)
            {
                throw new Exception("You forgot to add player on the level");
            }

            return new MapInfo(map, player, creatures);
        }
    }
}