using System.Collections.Generic;
using Model;

namespace View.AnimationsForCreatures
{
    public static class AnimationsForBox
    {
        public static readonly Dictionary<(MovementConditions, Direction), string> AnimationForBox =
            new Dictionary<(MovementConditions, Direction), string>
            {
                {(MovementConditions.Default, Direction.Front), @"Sprites\boxAnimations"}
            };
    }
}