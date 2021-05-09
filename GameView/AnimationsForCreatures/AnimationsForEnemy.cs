using System.Collections.Generic;
using Model;

namespace View.AnimationsForCreatures
{
    public static class AnimationsForEnemy
    {
        public static readonly Dictionary<(MovementConditions, Direction), string> AnimationForPlayer =
            new Dictionary<(MovementConditions, Direction), string>
            {
                {(MovementConditions.Standing, Direction.Right), @"Sprites\enemyAnimations\standingRight"},
                {(MovementConditions.Standing, Direction.Left), @"Sprites\enemyAnimations\standingLeft"},
                
                {(MovementConditions.Running,Direction.Right), @"Sprites\enemyAnimations\runningRight"},
                {(MovementConditions.Running,Direction.Left), @"Sprites\enemyAnimations\runningLeft"},
                
                {(MovementConditions.Attacking, Direction.Right), @"Sprites\enemyAnimations\attackingRight"},
                {(MovementConditions.Attacking, Direction.Left), @"Sprites\enemyAnimations\attackingLeft"},
                
                {(MovementConditions.Dying, Direction.Right), @"Sprites\enemyAnimations\dieRight"},
                {(MovementConditions.Dying, Direction.Left), @"Sprites\enemyAnimations\dieLeft"}
            };
    }
}