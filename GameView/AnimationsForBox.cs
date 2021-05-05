using System.Collections.Generic;
using GameModel;

namespace View
{
    public static class AnimationsForBox
    {
        public static readonly Dictionary<MovementConditions, string> AnimationForBox =
            new Dictionary<MovementConditions, string>
            {
                {MovementConditions.Default, @"Sprites\boxAnimations"}
            };
    }
}