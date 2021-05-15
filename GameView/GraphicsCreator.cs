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
                    GetCoordinationForCreatureInPixels(map.GetCreatureLocation(creature), 40));
        }

        private static Point GetCoordinationForCreatureInPixels(Point coordinates, int cellSize) //улучшить метод
        {
            return new Point(coordinates.X * cellSize, coordinates.Y * cellSize);
        }
    }
}