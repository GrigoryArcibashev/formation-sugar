using System.Collections.Generic;
using System.Drawing;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class GraphicsCreator
    {
        public static void CreateGraphic(
            Graphics graphics,
            Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures,  
            IEnumerable<ICreature> creatures)
        {
            foreach (var creature in creatures)
            {
                graphics.DrawImage(animationsForCreatures[creature][creature.MovementCondition].Current, creature.Location);
            }
        }
    }
}