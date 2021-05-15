﻿using System.Linq;
using Model.Creatures;

namespace Model
{
    public static class CreatureLocationAndConditionsUpdater
    {
        public static void UpdateLocationAndCondition(GameMap map)
        {
            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
            {
                switch (creature.MovementCondition)
                {
                    case MovementConditions.Running:
                        map.MoveCreature(creature, creature.Direction);
                        break;

                    case MovementConditions.Jumping:
                        if (map.MoveCreature(creature, Direction.Up))
                            map.MoveCreature(creature, creature.Direction);
                        break;

                    case MovementConditions.Falling:
                        if (map.MoveCreature(creature, Direction.Down))
                            map.MoveCreature(creature, creature.Direction);
                        break;
                    
                    case MovementConditions.Attacking:
                        map.Attack((IAttackingCreature)creature);
                        break;
                }
            }
        }
    }
}