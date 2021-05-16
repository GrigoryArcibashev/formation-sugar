using System.Collections.Generic;
using Model;

namespace View.AnimationsForCreatures
{
    public static class AnimationsForPlayer
    {
        public static readonly Dictionary<(MovementConditions, Direction), string> AnimationForPlayer =
            new Dictionary<(MovementConditions, Direction), string>
            {
                {(MovementConditions.Standing, Direction.Right), @"Sprites\playerAnimations\standingRight"},
                {(MovementConditions.Standing, Direction.Left), @"Sprites\playerAnimations\standingLeft"},
                
                {(MovementConditions.Running,Direction.Right), @"Sprites\playerAnimations\runningRight"},
                {(MovementConditions.Running,Direction.Left), @"Sprites\playerAnimations\runningLeft"},

                {(MovementConditions.Jumping, Direction.Right), @"Sprites\playerAnimations\jumpingRight"},
                {(MovementConditions.Jumping, Direction.Left), @"Sprites\playerAnimations\jumpingLeft"},
                {(MovementConditions.Jumping, Direction.NoMovement), @"Sprites\playerAnimations\jumpingAndFallingNoMovement"},
                
                {(MovementConditions.Falling, Direction.Right), @"Sprites\playerAnimations\jumpingRight"},
                {(MovementConditions.Falling, Direction.Left), @"Sprites\playerAnimations\jumpingLeft"},
                {(MovementConditions.Falling, Direction.NoMovement), @"Sprites\playerAnimations\jumpingAndFallingNoMovement"},
                
                {(MovementConditions.Attacking, Direction.Right), @"Sprites\playerAnimations\attackingRight"},
                {(MovementConditions.Attacking, Direction.Left), @"Sprites\playerAnimations\attackingLeft"},
                
                {(MovementConditions.Dying, Direction.Right), @"Sprites\playerAnimations\dieRight"},
                {(MovementConditions.Dying, Direction.Left), @"Sprites\playerAnimations\dieLeft"}
            };
    }
}