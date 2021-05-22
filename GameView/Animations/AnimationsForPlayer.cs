using System.Collections.Generic;
using Model;

namespace View.Animations
{
    public static class AnimationsForPlayer
    {
        public static readonly Dictionary<(MovementCondition, Direction), string> AnimationForPlayer =
            new Dictionary<(MovementCondition, Direction), string>
            {
                {(MovementCondition.Standing, Direction.Right), @"Sprites\playerAnimations\standingRight"},
                {(MovementCondition.Standing, Direction.Left), @"Sprites\playerAnimations\standingLeft"},

                {(MovementCondition.Running, Direction.Right), @"Sprites\playerAnimations\runningRight"},
                {(MovementCondition.Running, Direction.Left), @"Sprites\playerAnimations\runningLeft"},

                {(MovementCondition.Jumping, Direction.Right), @"Sprites\playerAnimations\jumpingRight"},
                {(MovementCondition.Jumping, Direction.Left), @"Sprites\playerAnimations\jumpingLeft"},
                {(MovementCondition.Jumping, Direction.NoMovement), @"Sprites\playerAnimations\jumpingAndFallingNoMovement"},

                {(MovementCondition.Falling, Direction.Right), @"Sprites\playerAnimations\jumpingRight"},
                {(MovementCondition.Falling, Direction.Left), @"Sprites\playerAnimations\jumpingLeft"},
                {(MovementCondition.Falling, Direction.NoMovement), @"Sprites\playerAnimations\jumpingAndFallingNoMovement"},

                {(MovementCondition.Attacking, Direction.Right), @"Sprites\playerAnimations\attackingRight"},
                {(MovementCondition.Attacking, Direction.Left), @"Sprites\playerAnimations\attackingLeft"},

                {(MovementCondition.Dying, Direction.Right), @"Sprites\playerAnimations\dieRight"},
                {(MovementCondition.Dying, Direction.Left), @"Sprites\playerAnimations\dieLeft"}
            };
    }
}