using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;

namespace Model
{
    public class GameMap
    {
        private ICreature[,] map;
        private Dictionary<ICreature, Point> creaturesLocations;
        public int Score { get; private set; }
        public int Width => map.GetLength(0);
        private int Height => map.GetLength(1);

        public List<ICreature> ListOfCreatures { get; private set; }
        public Player Player { get; private set; }
        public ICreature this[int x, int y] => map[x, y];

        public GameMap()
        {
            LoadNextMap();
        }

        public Point GetCreatureLocation(ICreature creature)
        {
            return creaturesLocations[creature];
        }

        public bool MoveCreature(IMovingCreature creature, Direction direction)
        {
            return direction switch
            {
                Direction.Right => MoveCreatureToSide(creature, direction),
                Direction.Left => MoveCreatureToSide(creature, direction),
                Direction.Up => MoveCreatureUp((IJumpingCreature) creature),
                Direction.Down => MoveCreatureDown((IJumpingCreature) creature),
                Direction.NoMovement => true,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public bool Attack(IAttackingCreature creature)
        {
            var creatureCoordinates = GetCreatureLocation(creature);
            var enemiesCoordinates = new[]
            {
                creatureCoordinates + new Size(0, 1),
                creatureCoordinates + new Size(0, -1),
                creatureCoordinates + new Size(creature.Direction is Direction.Right ? 1 : -1, 0)
            };

            return Attack(creature, enemiesCoordinates);
        }

        public void CheckCreaturesForFalling()
        {
            foreach (var creature in ListOfCreatures.OfType<IJumpingCreature>())
            {
                if (creature.IsFalling() || creature.IsJumping() || !IsThereNothingUnderCreature(creature))
                    continue;
                creature.ResetVelocityToZero();
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, Direction.NoMovement);
            }
        }

        public void MakeEnemiesAttackingOrRunning()
        {
            var playerCoordinates = GetCreatureLocation(Player);
            foreach (var enemy in ListOfCreatures.OfType<Enemy>())
            {
                if (Player.IsDead())
                {
                    enemy.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, enemy.Direction);
                    return;
                }

                var dx = GetCreatureLocation(enemy).X - playerCoordinates.X;
                if (Math.Abs(dx) > 10)
                {
                    if (enemy.MovementCondition != MovementConditions.Standing)
                        enemy.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, enemy.Direction);
                    continue;
                }

                enemy.ChangeMovementConditionAndDirectionTo(
                    Math.Abs(dx) > 1 ? MovementConditions.Running : MovementConditions.Attacking,
                    dx > 0 ? Direction.Left : Direction.Right);
            }
        }

        public void RemoveCreaturesFromMapIfTheyAreDead()
        {
            var deadEnemies = ListOfCreatures
                .OfType<ICreatureWithHealth>()
                .Where(enemy => enemy.MovementCondition is MovementConditions.Dying && !(enemy is Player))
                .ToList();

            while (deadEnemies.Count > 0)
            {
                var enemy = deadEnemies[0];
                var enemyLocation = GetCreatureLocation(enemy);
                map[enemyLocation.X, enemyLocation.Y] = null;
                ListOfCreatures.Remove(enemy);
                creaturesLocations.Remove(enemy);
                deadEnemies.Remove(enemy);
            }
        }

        public void LoadNextMap()
        {
            var mapInfo = MapCreator.GetNextMap();
            ListOfCreatures = mapInfo.ListOfCreatures;
            Player = mapInfo.Player;
            map = mapInfo.Map;
            creaturesLocations = GetCreaturesLocations();
        }

        private bool Attack(IAttackingCreature creature, IEnumerable<Point> enemiesCoordinates)
        {
            var isEnemyAttacked = false;
            foreach (var enemyCoordinates in enemiesCoordinates)
            {
                if (!IsAttackPossible(enemyCoordinates))
                    continue;
                var enemy = (ICreatureWithHealth) map[enemyCoordinates.X, enemyCoordinates.Y];
                enemy.ChangeHealthBy(creature.DamageValue);
                if (enemy is Chest chest)
                    Score += chest.Score;
                isEnemyAttacked = true;
            }

            return isEnemyAttacked;
        }

        private bool IsAttackPossible(Point enemyCoordinates)
        {
            return IsPointInBounds(enemyCoordinates)
                   && map[enemyCoordinates.X, enemyCoordinates.Y] is ICreatureWithHealth
                   && !(map[enemyCoordinates.X, enemyCoordinates.Y].MovementCondition is MovementConditions.Dying);
        }

        private bool IsThereNothingUnderCreature(IJumpingCreature creature)
        {
            return IsMovementPossible(creature, creaturesLocations[creature] + new Size(0, 1));
        }

        private bool MoveCreatureToSide(IMovingCreature creature, Direction direction)
        {
            var shift = direction switch
            {
                Direction.Right => new Size(1, 0),
                Direction.Left => new Size(-1, 0),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    "The direction of movement is specified incorrectly")
            };
            return MoveCreatureOn(creature, creaturesLocations[creature] + shift);
        }

        private bool MoveCreatureUp(IJumpingCreature creature)
        {
            if (!MoveCreatureOn(creature, creaturesLocations[creature] + new Size(0, -creature.Velocity))
                || creature.Velocity <= 0)
            {
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, creature.Direction);
                creature.ResetVelocityToZero();
                return false;
            }

            creature.ReduceVelocity();
            return true;
        }

        private bool MoveCreatureDown(IJumpingCreature creature)
        {
            if (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(0, creature.Velocity)))
            {
                creature.IncreaseVelocity();
                return true;
            }

            while (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(0, 1)))
            {
            }

            if (creature.Direction is Direction.NoMovement)
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, Direction.Right);
            creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, creature.Direction);
            creature.RecoverVelocity();
            return false;
        }

        private bool MoveCreatureOn(IMovingCreature creature, Point targetLocation)
        {
            if (!IsMovementPossible(creature, targetLocation))
                return false;
            map[creaturesLocations[creature].X, creaturesLocations[creature].Y] = null;
            map[targetLocation.X, targetLocation.Y] = creature;
            creaturesLocations[creature] = targetLocation;
            return true;
        }

        private bool IsMovementPossible(IMovingCreature creature, Point target)
        {
            var topLeftCorner = new Point(
                Math.Min(target.X, creaturesLocations[creature].X),
                Math.Min(target.Y, creaturesLocations[creature].Y));
            var bottomRightCorner = new Point(
                Math.Max(target.X, creaturesLocations[creature].X),
                Math.Max(target.Y, creaturesLocations[creature].Y));
            return IsPointInBounds(target) && IsMapPieceEmpty(creature, topLeftCorner, bottomRightCorner);
        }

        private bool IsMapPieceEmpty(IMovingCreature creature, Point topLeftCorner, Point bottomRightCorner)
        {
            for (var x = topLeftCorner.X; x <= bottomRightCorner.X; x++)
            for (var y = topLeftCorner.Y; y <= bottomRightCorner.Y; y++)
                if (map[x, y] != creature && map[x, y] != null)
                    return false;
            return true;
        }

        private bool IsPointInBounds(Point point)
        {
            return point.X >= 0
                   && point.X < Width
                   && point.Y >= 0
                   && point.Y < Height;
        }

        private Dictionary<ICreature, Point> GetCreaturesLocations()
        {
            var locations = new Dictionary<ICreature, Point>();
            foreach (var creature in ListOfCreatures)
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    if (map[x, y] == creature)
                        locations.Add(creature, new Point(x, y));
            return locations;
        }
    }
}