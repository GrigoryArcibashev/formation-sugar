using System.Collections.Generic;
using System.IO;
using System.Linq;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class AnimationsForCreatures
    {
        private static readonly Dictionary<MovementConditions, string> AnimationForPlayer =
            new Dictionary<MovementConditions, string>
            {
                {MovementConditions.StandingRight, @"Sprites\playerAnimations\standingRight"},
                {MovementConditions.RunningRight, @"Sprites\playerAnimations\runningRight"},
                {MovementConditions.SittingRight, @"Sprites\playerAnimations\sittingRight"},
                {MovementConditions.JumpingRight, @"Sprites\playerAnimations\jumpingRight"},
                {MovementConditions.AttackingRight, @"Sprites\playerAnimations\attackingRight"},
                {MovementConditions.DieRight, @"Sprites\playerAnimations\dieRight"},
                {MovementConditions.StandingLeft, @"Sprites\playerAnimations\standingLeft"},
                {MovementConditions.RunningLeft, @"Sprites\playerAnimations\runningLeft"},
                {MovementConditions.SittingLeft, @"Sprites\playerAnimations\sittingLeft"},
                {MovementConditions.JumpingLeft, @"Sprites\playerAnimations\jumpingLeft"},
                {MovementConditions.AttackingLeft, @"Sprites\playerAnimations\attackingLeft"},
                {MovementConditions.DieLeft, @"Sprites\playerAnimations\dieLeft"}
            };

        private static readonly Dictionary<MovementConditions, string> AnimationForBox =
            new Dictionary<MovementConditions, string>
            {
                {MovementConditions.Default, @"Sprites\simpleGrass"}
            };

        public static Dictionary<MovementConditions, Animation> GetAnimationFor(ICreature creature)
        {
            if (creature.GetType() == typeof(Player))
            {
                return GetAnimationForCreature(AnimationForPlayer);
            }

            if (creature.GetType() == typeof(Box))
            {
                return GetAnimationForCreature(AnimationForBox);
            }

            return default;
        }

        private static Dictionary<MovementConditions, Animation> GetAnimationForCreature(Dictionary<MovementConditions, string> creature)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            return creature.ToDictionary(
                movementConditionAndSpritesForIt => movementConditionAndSpritesForIt.Key,
                movementConditionAndSpritesForIt =>
                    new Animation(new DirectoryInfo(Path.Combine(currentDirectory!,
                        movementConditionAndSpritesForIt.Value))));
        }
    }
}