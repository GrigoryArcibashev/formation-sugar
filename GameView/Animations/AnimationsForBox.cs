using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForBox
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForBox =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Default, Direction.NoMovement), @"Sprites\boxAnimations"}
            };
    }
}