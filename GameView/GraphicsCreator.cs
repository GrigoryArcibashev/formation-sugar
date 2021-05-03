﻿using System.Collections.Generic;
using System.Drawing;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class GraphicsCreator
    {
        public static void CreateGraphic(
            Graphics graphics,
            Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures,
            GameMap map)
        {
            foreach (var creature in map.ListOfCreatures)
                graphics.DrawImage(
                    animationsForCreatures[creature][creature.MovementCondition].Current,
                    ScaleCoordinatesInPercents(map.GetCreatureLocation(creature), 300));
        }

        private static Point ScaleCoordinatesInPercents(Point coordinates, double percent)
        {
            var ds = (int) (1 + percent / 100);
            return new Point(coordinates.X * ds, coordinates.Y * ds);
        }
    }
}