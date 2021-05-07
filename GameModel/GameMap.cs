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

        public void MoveCreature(IMovingCreature creature, Direction horizontalDirection, Direction verticalDirection)
        {
            switch (verticalDirection)
            {
                case Direction.NoMovement:
                    MoveCreatureToSide(creature, horizontalDirection);
                    break;
                case Direction.Up:
                    MoveCreatureUp(creature, horizontalDirection);
                    break;
                case Direction.Down:
                    MoveCreatureDown(creature, horizontalDirection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(verticalDirection),
                        "The direction of movement is specified incorrectly");
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
            return IsMovementPossible(creaturesLocations[creature] + new Size(0, 1));
        }

        private void MoveCreatureToSide(IMovingCreature creature, Direction horizontalShift)
        {
            var shift = horizontalShift switch
            {
                Direction.Right => new Size(1, 0),
                Direction.Left => new Size(-1, 0),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(horizontalShift),
                    "The direction of movement is specified incorrectly")
            };
            
            MoveCreatureOn(creature, creaturesLocations[creature] + shift);
        }

        private void MoveCreatureUp(IMovingCreature creature, Direction horizontalShift)
        {
            var shift = horizontalShift switch
            {
                Direction.Right => new Size(1, -creature.Velocity),
                Direction.Left => new Size(-1, -creature.Velocity),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(horizontalShift),
                    "The direction of movement is specified incorrectly")
            };
            
            if (MoveCreatureOn(creature, creaturesLocations[creature] + shift))
                creature.ReduceVelocity();
            
            if (creature.Velocity <= 0)
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, creature.Direction);
        }

        private void MoveCreatureDown(IMovingCreature creature, Direction horizontalShift)
        {
            var shift = horizontalShift switch
            {
                Direction.NoMovement => new Size(0, creature.Velocity),
                Direction.Right => new Size(1, creature.Velocity),
                Direction.Left => new Size(-1, creature.Velocity),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(horizontalShift),
                    "The direction of movement is specified incorrectly")
            };
            
            if (MoveCreatureOn(creature, creaturesLocations[creature] + shift))
                creature.IncreaseVelocity();
        }

        private bool MoveCreatureOn(IMovingCreature creature, Point targetLocation)
        {
            if (!IsMovementPossible(targetLocation))
            {
                if (creature.IsJumping())
                {
                    creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, creature.Direction);
                    creature.ResetVelocityToZero();
                }

                else if (creature.IsFalling())
                {
                    if (creature.Direction is Direction.NoMovement)
                    {
                        creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, Direction.Right);
                    }

                    creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, creature.Direction);
                    creature.RecoverVelocity();
                }

                return false;
            }

            map[creaturesLocations[creature].X, creaturesLocations[creature].Y] = null;
            map[targetLocation.X, targetLocation.Y] = creature;
            creaturesLocations[creature] = targetLocation;
            return true;
        }

        private bool IsMovementPossible(Point target)
        {
            return IsPointInBounds(target) && map[target.X, target.Y] == null;
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