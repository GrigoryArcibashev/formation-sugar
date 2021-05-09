using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Model.Creatures;

namespace Model
{
    public class GameMap
    {
        private ICreature[,] map;
        private Dictionary<ICreature, Point> creaturesLocations;
        private int Width => map.GetLength(0);
        private int Height => map.GetLength(1);

        public List<ICreature> ListOfCreatures { get; private set; }
        public Player Player { get; private set; }

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
            switch (direction)
            {
                case Direction.Right:
                    return MoveCreatureToSide(creature, direction);
                case Direction.Left:
                    return MoveCreatureToSide(creature, direction);
                case Direction.Up:
                    return MoveCreatureUp(creature);
                case Direction.Down:
                    return MoveCreatureDown(creature);
                case Direction.NoMovement:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void CheckCreaturesForFalling()
        {
            foreach (var creature in ListOfCreatures.OfType<IMovingCreature>())
            {
                if (creature.IsFalling() || creature.IsJumping() || !IsThereNothingUnderCreature(creature))
                    continue;
                creature.ResetVelocityToZero();
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, Direction.NoMovement);
            }
        }

        private bool IsThereNothingUnderCreature(IMovingCreature creature)
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

        private bool MoveCreatureUp(IMovingCreature creature)
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

        private bool MoveCreatureDown(IMovingCreature creature)
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
            {
                for (var y = topLeftCorner.Y; y <= bottomRightCorner.Y; y++)
                {
                    if (map [x, y] != creature && map[x, y] != null)
                        return false;
                }
            }

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

        private void LoadNextMap()
        {
            var mapInfo = MapCreator.GetNextMap();
            ListOfCreatures = mapInfo.ListOfCreatures;
            Player = mapInfo.Player;
            map = mapInfo.Map;
            creaturesLocations = GetCreaturesLocations();
        }
    }
}