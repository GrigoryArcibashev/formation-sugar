using System.Collections.Generic;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class AnimationsForBox
    {
        public static readonly Dictionary<MovementConditions, string> AnimationForBox =
            new Dictionary<MovementConditions, string>
            {
                {MovementConditions.Default, @"Sprites\simpleGrass"}
            };
    }
}