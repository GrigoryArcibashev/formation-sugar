using System;
using System.Collections.Generic;
using System.Drawing;

namespace formation_sugar.GameModel
{
    public static class MapCreator
    {
        public static MapInfo CreateMap(IEnumerable<string> level)
        {
            Player player = default;
            var map = new List<ICreature>();
            
            foreach (var line in level)
            {
                var parts = line.Split();
                var coordinates = new Point(int.Parse(parts[1]), int.Parse(parts[2]));

                switch (parts[0])
                {
                    case "P":
                        player = new Player(coordinates, 100, 2);
                        map.Add(player);
                        break;

                    case "G":
                        map.Add(new Box(coordinates));
                        break;
                }
            }

            if (player == default)
            {
                throw new Exception("You forgot to add player on the level");
            }
            
            return new MapInfo(player, map);
        }
    }
}