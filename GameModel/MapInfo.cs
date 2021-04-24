using System.Collections.Generic;

namespace formation_sugar.GameModel
{
    public class MapInfo
    {
        public readonly Player Player;
        public readonly List<ICreature> Map;

        public MapInfo(Player player, List<ICreature> map)
        {
            Player = player;
            Map = map;
        }
    }
}