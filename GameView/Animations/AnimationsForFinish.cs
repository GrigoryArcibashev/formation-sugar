using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForFinish
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForFinish =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Default, Direction.NoMovement), @"Sprites\finishAnimations"},
                {(MovementCondition.Dying, Direction.NoMovement), @"Sprites\finishAnimations"}
            };
    }
}