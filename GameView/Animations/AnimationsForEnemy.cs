using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForEnemy
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForEnemy =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Standing, Direction.Right), @"Sprites\enemyAnimations\standingRight"},
                {(MovementCondition.Standing, Direction.Left), @"Sprites\enemyAnimations\standingLeft"},

                {(MovementCondition.Running, Direction.Right), @"Sprites\enemyAnimations\runningRight"},
                {(MovementCondition.Running, Direction.Left), @"Sprites\enemyAnimations\runningLeft"},

                {(MovementCondition.Attacking, Direction.Right), @"Sprites\enemyAnimations\attackingRight"},
                {(MovementCondition.Attacking, Direction.Left), @"Sprites\enemyAnimations\attackingLeft"},

                {(MovementCondition.Dying, Direction.Right), @"Sprites\enemyAnimations\dieRight"},
                {(MovementCondition.Dying, Direction.Left), @"Sprites\enemyAnimations\dieLeft"}
            };
    }
}