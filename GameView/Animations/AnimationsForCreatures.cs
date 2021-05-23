using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;

namespace View.Animations
{
    public static class AnimationsForCreatures
    {
        public static Dictionary<(MovementCondition, Direction), Animation> GetAnimationFor(ICreature creature)
        {
            return creature switch
            {
                Player _ => GetAnimationForCreature(AnimationsForPlayer.AnimationForPlayer),
                BossEnemy _ => GetAnimationForCreature(AnimationsForBossEnemy.AnimationForBossEnemy),
                Box _ => GetAnimationForCreature(AnimationsForBox.AnimationForBox),
                Enemy _ => GetAnimationForCreature(AnimationsForEnemy.AnimationForEnemy),
                Chest _ => GetAnimationForCreature(AnimationsForChest.AnimationForChest),
                Finish _ => GetAnimationForCreature(AnimationsForFinish.AnimationForFinish),
                _ => default
            };
        }

        private static Dictionary<(MovementCondition, Direction), Animation> GetAnimationForCreature(
            Dictionary<(MovementCondition, Direction), string> creature)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            return creature.ToDictionary(
                movementConditionAndSpritesForIt => movementConditionAndSpritesForIt.Key,
                movementConditionAndSpritesForIt => new Animation(
                    new DirectoryInfo(
                        Path.Combine(
                            currentDirectory ?? throw new Exception("Unable to load animation"),
                            movementConditionAndSpritesForIt.Value))));
        }
    }
}