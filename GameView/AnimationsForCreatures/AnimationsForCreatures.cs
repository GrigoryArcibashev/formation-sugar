﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using Model.Creatures;

namespace View.AnimationsForCreatures
{
    public static class AnimationsForCreatures
    {
        public static Dictionary<(MovementConditions, Direction), Animation> GetAnimationFor(ICreature movingCreature)
        {
            return movingCreature switch
            {
                Player _ => GetAnimationForCreature(AnimationsForPlayer.AnimationForPlayer),
                Box _ => GetAnimationForCreature(AnimationsForBox.AnimationForBox),
                Enemy _ => GetAnimationForCreature(AnimationsForEnemy.AnimationForEnemy),
                _ => default
            };
        }

        private static Dictionary<(MovementConditions, Direction), Animation> GetAnimationForCreature(
            Dictionary<(MovementConditions, Direction), string> creature)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            return creature.ToDictionary(
                movementConditionAndSpritesForIt => movementConditionAndSpritesForIt.Key,
                movementConditionAndSpritesForIt => new Animation(
                    new DirectoryInfo(
                        Path.Combine(
                            currentDirectory ?? throw new Exception("Не удается загрузить анимацию"),
                            movementConditionAndSpritesForIt.Value))));
        }
    }
}