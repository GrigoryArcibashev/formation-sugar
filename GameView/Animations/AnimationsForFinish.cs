using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForFinish
    {
        public static readonly Dictionary<(MovementConditions, Direction), string> AnimationForFinish =
            new Dictionary<(MovementConditions, Direction), string>
            {
                {(MovementConditions.Default, Direction.NoMovement), @"Sprites\finishAnimations"},
                {(MovementConditions.Dying, Direction.NoMovement), @"Sprites\finishAnimations"}
            };
    }
}