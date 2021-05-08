using System.Collections.Generic;
using System.Drawing;
using Model;
using Model.Creatures;

namespace View
{
    public static class GraphicsCreator
    {
        public static void CreateGraphic(
            Graphics graphics,
            Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>> animationsForCreatures,
            GameMap map)
        {
            foreach (var creature in map.ListOfCreatures)
                graphics.DrawImage(
                    animationsForCreatures[creature][(creature.MovementCondition, creature.Direction)].Current,
                    ScaleCoordinatesInPercents(map.GetCreatureLocation(creature), 200));
        }

        private static Point ScaleCoordinatesInPercents(Point coordinates, double percent) //улучшить метод
        {
            var ds = (int) (1 + percent / 100);
            return new Point(coordinates.X * ds, coordinates.Y * ds);
        }
    }
}