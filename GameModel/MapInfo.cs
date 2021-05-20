using System.Collections.Generic;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;

namespace Model
{
    public class MapInfo
    {
        public readonly ICreature[,] Map;
        public readonly List<ICreature> ListOfCreatures;
        public readonly Player Player;
        public readonly Finish Finish;

        public MapInfo(ICreature[,] map, List<ICreature> listOfCreatures,Player player, Finish finish)
        {
            Map = map;
            ListOfCreatures = listOfCreatures;
            Player = player;
            Finish = finish;
        }
    }
}