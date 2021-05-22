using System.Linq;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;

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
                    case MovementCondition.Running:
                        map.MoveCreature(creature, creature.Direction);
                        break;

                    case MovementCondition.Jumping:
                        if (map.MoveCreature(creature, Direction.Up))
                            map.MoveCreature(creature, creature.Direction);
                        break;

                    case MovementCondition.Falling:
                        if (map.MoveCreature(creature, Direction.Down))
                            map.MoveCreature(creature, creature.Direction);
                        break;

                    case MovementCondition.Attacking:
                        if (!map.Attack((IAttackingCreature) creature) && !(creature is Player))
                            creature.ChangeMovementConditionAndDirectionTo(MovementCondition.Standing, creature.Direction);
                        break;
                }
            }
        }
    }
}