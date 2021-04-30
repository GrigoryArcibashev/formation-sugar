using System.Collections.Generic;

namespace formation_sugar.GameModel
{
    public class MapInfo
    {
        public readonly Player Player;
        public readonly List<ICreature> CreaturesToDraw;
        public readonly ICreature[,] Map;

        public MapInfo(ICreature[,] map, Player player, List<ICreature> creaturesToDraw)
        {
            Map = map;
            Player = player;
            CreaturesToDraw = creaturesToDraw;
        }
    }
}