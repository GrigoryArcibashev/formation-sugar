using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForChest
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForChest =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Default, Direction.NoMovement), @"Sprites\chestAnimations"},
                {(MovementCondition.Dying, Direction.NoMovement), @"Sprites\chestAnimations"}
            };
    }
}