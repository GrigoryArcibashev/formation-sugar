using System.Collections.Generic;
using System.Drawing;
using Model;
using Model.Creatures.CreatureInterfaces;

namespace View
{
    public static class GraphicsCreator
    {
        public static void CreateGraphicForCreatures(
            Graphics graphics,
            Dictionary<ICreature, Dictionary<(MovementCondition, Direction), Animation>> animationsForCreatures,
            GameMap map)
        {
            foreach (var creature in map.ListOfCreatures)
                graphics.DrawImage(
                    animationsForCreatures[creature][(creature.MovementCondition, creature.Direction)].Current,
                    GetCoordinationForCreatureInPixels(map.GetCreatureLocation(creature), 40));
        }

        public static void CreateGraphicForPlayersHealth(Graphics graphics, Animation animation)
        {
            graphics.DrawImage(animation.Current, new Point(5, 5));
        }

        private static Point GetCoordinationForCreatureInPixels(Point coordinates, int cellSizeInPixels)
        {
            return new Point(coordinates.X * cellSizeInPixels, coordinates.Y * cellSizeInPixels);
        }
    }
}