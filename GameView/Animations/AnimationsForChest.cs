using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForChest
    {
        public static readonly Dictionary<(MovementConditions, Direction), string> AnimationForChest =
            new Dictionary<(MovementConditions, Direction), string>
            {
                {(MovementConditions.Default, Direction.NoMovement), @"Sprites\chestAnimations"},
                {(MovementConditions.Dying, Direction.NoMovement), @"Sprites\chestAnimations"}
            };
    }
}