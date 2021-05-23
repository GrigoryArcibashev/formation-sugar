using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForBossEnemy
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForBossEnemy =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Standing, Direction.Right), @"Sprites\boss\standingRight"},
                {(MovementCondition.Standing, Direction.Left), @"Sprites\boss\standingLeft"},

                {(MovementCondition.Running, Direction.Right), @"Sprites\boss\runningRight"},
                {(MovementCondition.Running, Direction.Left), @"Sprites\boss\runningLeft"},

                {(MovementCondition.Attacking, Direction.Right), @"Sprites\boss\attackingRight"},
                {(MovementCondition.Attacking, Direction.Left), @"Sprites\boss\attackingLeft"},

                {(MovementCondition.Dying, Direction.Right), @"Sprites\boss\dieRight"},
                {(MovementCondition.Dying, Direction.Left), @"Sprites\boss\dieLeft"}
            };
    }
}