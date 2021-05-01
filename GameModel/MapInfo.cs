using System.Collections.Generic;

namespace formation_sugar.GameModel
{
    public class MapInfo
    {
        public readonly Player Player;
        public readonly List<ICreature> ListOfCreatures;
        public readonly ICreature[,] Map;

        public MapInfo(ICreature[,] map, Player player, List<ICreature> listOfCreatures)
        {
            Map = map;
            Player = player;
            ListOfCreatures = listOfCreatures;
        }
    }
}