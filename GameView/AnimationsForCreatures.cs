using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using formation_sugar.GameModel;

namespace formation_sugar.View
{
    public static class AnimationsForCreatures
    {
        public static Dictionary<MovementConditions, Animation> GetAnimationFor(ICreature movingCreature)
        {
            switch (movingCreature.GetTypeAsString())
            {
                case "Player":
                    return GetAnimationForCreature(AnimationsForPlayer.AnimationForPlayer);
                
                case "Box":
                    return GetAnimationForCreature(AnimationsForBox.AnimationForBox);
                
                default:
                    return default;
            }
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