using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Model.Creatures;

namespace Model
{
    public class GameMap
    {
        private readonly List<FileInfo> levels;
        private DirectoryInfo fullPathToLevels;
        private ICreature[,] map;
        private readonly Dictionary<ICreature, Point> creaturesLocations;

        public List<ICreature> ListOfCreatures { get; private set; }
        public Player Player { get; private set; }
        public int Width => map.GetLength(0);
        public int Height => map.GetLength(1);

        private GameMap()
        {
            levels = new List<FileInfo>();
            ListOfCreatures = new List<ICreature>();
            creaturesLocations = new Dictionary<ICreature, Point>();
            AddLevels();
        }

        public GameMap(int levelNumber) : this()
        {
            CreateMap(levelNumber);
            FillLocations();
        }

        public GameMap(string levelName) : this()
        {
            CreateMap(levelName);
            FillLocations();
        }

        public ICreature this[int x, int y] => map[x, y];

        public Point GetCreatureLocation(ICreature creature)
        {
            return creaturesLocations[creature];
        }

        [MoveCreature]
        public void MoveCreatureToRight(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creaturesLocations[creature] + new Size(1, 0));
        }

        [MoveCreature]
        public void MoveCreatureToLeft(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creaturesLocations[creature] + new Size(-1, 0));
        }

        [MoveCreature]
        [MoveCreatureDiagonally]
        public void MoveCreatureToRightAndToUp(IMovingCreature creature)
        {
            if (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(1, -creature.Velocity)))
                creature.ReduceVelocity();
            
            if (creature.Velocity <= 0)
                creature.ChangeConditionToFalling();
        }

        [MoveCreature]
        [MoveCreatureDiagonally]
        public void MoveCreatureToLeftAndToUp(IMovingCreature creature)
        {
            if (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(-1, -creature.Velocity)))
                creature.ReduceVelocity();
            
            if (creature.Velocity <= 0)
                creature.ChangeConditionToFalling();
        }

        [MoveCreature]
        [MoveCreatureDiagonally]
        public void MoveCreatureToRightAndToDown(IMovingCreature creature)
        {
            if (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(1, creature.Velocity)))
                creature.IncreaseVelocity();
        }

        [MoveCreature]
        [MoveCreatureDiagonally]
        public void MoveCreatureToLeftAndToDown(IMovingCreature creature)
        {
            if (MoveCreatureOn(creature, creaturesLocations[creature] + new Size(-1, creature.Velocity)))
                creature.IncreaseVelocity();
        }

        [MoveCreature]
        public void MoveCreatureToDown(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creaturesLocations[creature] + new Size(0, creature.Velocity));
            creature.IncreaseVelocity();
        }

        public void CheckCreaturesForFalling()
        {
            foreach (var creature in ListOfCreatures.OfType<IMovingCreature>())
            {
                if (creature.IsFalling() || creature.IsJumping() || !IsThereNothingUnderCreature(creature))
                    continue;
                creature.ResetVelocityToZero();
                creature.ChangeConditionToFallingDown();
            }
        }

        private bool IsThereNothingUnderCreature(IMovingCreature creature)
        {
            return IsMovementPossible(creature, creaturesLocations[creature] + new Size(0, 1));
        }

        private bool MoveCreatureOn(IMovingCreature creature, Point targetLocation)
        {
            if (!IsMovementPossible(creature, targetLocation))
            {
                if (creature.IsJumping())
                {
                    creature.ChangeConditionToFalling();
                    creature.ResetVelocityToZero();
                }
                
                else if (creature.IsFalling())
                {
                    creature.ChangeConditionToStanding();
                    creature.RecoverVelocity();
                }

                return false;
            }

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
            return IsPointInBounds(target) && IsMapPieceEmpty(topLeftCorner, bottomRightCorner);
        }

        // Тут стоит временный костыль Map[x, y] != Player
        // В дальнейшем его нужно будет обязательно убрать
        private bool IsMapPieceEmpty(Point topLeftCorner, Point bottomRightCorner)
        {
            for (var x = topLeftCorner.X; x <= bottomRightCorner.X; x++)
            {
                for (var y = topLeftCorner.Y; y <= bottomRightCorner.Y; y++)
                {
                    if (map [x, y] != Player && map[x, y] != null)
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

        private void CreateMap(int levelNumber)
        {
            var mapInfo = MapCreator.CreateMap(File.ReadAllLines(levels[levelNumber - 1].FullName));
            ListOfCreatures = mapInfo.ListOfCreatures;
            Player = mapInfo.Player;
            map = mapInfo.Map;
        }

        private void CreateMap(string levelName)
        {
            if (!levels
                .Select(fileInfo => fileInfo.Name)
                .Contains(levelName))
                throw new Exception($"Невозможно загрузить уровень\nУровень {levelName} отсутсвует в папке уровней");
            var mapInfo = MapCreator.CreateMap(
                File.ReadAllLines(
                    Path.Combine(
                        fullPathToLevels.FullName,
                        levelName)));
            ListOfCreatures = mapInfo.ListOfCreatures;
            Player = mapInfo.Player;
            map = mapInfo.Map;
        }

        private void AddLevels()
        {
            fullPathToLevels = new DirectoryInfo(
                Path.Combine(
                    new DirectoryInfo(
                        Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ??
                    throw new InvalidOperationException(),
                    "levels"));
            foreach (var levelName in fullPathToLevels.EnumerateFiles())
                levels.Add(levelName);
        }

        private void FillLocations()
        {
            foreach (var creature in ListOfCreatures)
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        if (map[x, y] == creature)
                        {
                            creaturesLocations.Add(creature, new Point(x, y));
                        }
                    }
                }
            }
        }
    }
}