using System.Collections.Generic;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class AnimationsForPlayer
    {
        public static readonly Dictionary<MovementConditions, string> AnimationForPlayer =
            new Dictionary<MovementConditions, string>
            {
                {MovementConditions.StandingRight, @"Sprites\playerAnimations\standingRight"},
                {MovementConditions.RunningRight, @"Sprites\playerAnimations\runningRight"},
                {MovementConditions.SittingRight, @"Sprites\playerAnimations\sittingRight"},
                {MovementConditions.JumpingRight, @"Sprites\playerAnimations\jumpingRight"},
                {MovementConditions.AttackingRight, @"Sprites\playerAnimations\attackingRight"},
                {MovementConditions.DieRight, @"Sprites\playerAnimations\dieRight"},
                {MovementConditions.FallingRight, @"Sprites\playerAnimations\fallingRight"},
                {MovementConditions.StandingLeft, @"Sprites\playerAnimations\standingLeft"},
                {MovementConditions.RunningLeft, @"Sprites\playerAnimations\runningLeft"},
                {MovementConditions.SittingLeft, @"Sprites\playerAnimations\sittingLeft"},
                {MovementConditions.JumpingLeft, @"Sprites\playerAnimations\jumpingLeft"},
                {MovementConditions.AttackingLeft, @"Sprites\playerAnimations\attackingLeft"},
                {MovementConditions.DieLeft, @"Sprites\playerAnimations\dieLeft"},
                {MovementConditions.FallingLeft, @"Sprites\playerAnimations\fallingLeft"}
            };
    }
}