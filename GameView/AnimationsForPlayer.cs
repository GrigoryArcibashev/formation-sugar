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
                {MovementConditions.StandingLeft, @"Sprites\playerAnimations\standingLeft"},
                {MovementConditions.RunningRight, @"Sprites\playerAnimations\runningRight"},
                {MovementConditions.RunningLeft, @"Sprites\playerAnimations\runningLeft"},
                {MovementConditions.SittingRight, @"Sprites\playerAnimations\sittingRight"},
                {MovementConditions.SittingLeft, @"Sprites\playerAnimations\sittingLeft"},
                {MovementConditions.JumpingRight, @"Sprites\playerAnimations\jumpingRight"},
                {MovementConditions.JumpingLeft, @"Sprites\playerAnimations\jumpingLeft"},
                {MovementConditions.FallingRight, @"Sprites\playerAnimations\jumpingRight"},
                {MovementConditions.FallingLeft, @"Sprites\playerAnimations\jumpingLeft"},
                {MovementConditions.FallingDown, @"Sprites\playerAnimations\jumpingRight"},
                {MovementConditions.AttackingRight, @"Sprites\playerAnimations\attackingRight"},
                {MovementConditions.AttackingLeft, @"Sprites\playerAnimations\attackingLeft"},
                {MovementConditions.DieRight, @"Sprites\playerAnimations\dieRight"},
                {MovementConditions.DieLeft, @"Sprites\playerAnimations\dieLeft"}
            };
    }
}