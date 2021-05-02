using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace formation_sugar.GameModel
{
    public class GameMap
    {
        private readonly List<FileInfo> levels;
        private DirectoryInfo fullPathToLevels;
        private ICreature[,] map;

        public List<ICreature> ListOfCreatures { get; private set; }
        public Player Player { get; private set; }
        public int Width => map.GetLength(0);
        public int Height => map.GetLength(1);

        public ICreature this[int x, int y]
        {
            get => map[x, y];
            set => map[x, y] = value;
        }

        private GameMap()
        {
            levels = new List<FileInfo>();
            ListOfCreatures = new List<ICreature>();
            AddLevels();
        }

        public GameMap(int levelNumber) : this()
        {
            CreateMap(levelNumber);
        }

        public GameMap(string levelName) : this()
        {
            CreateMap(levelName);
        }

        public void MoveCreatureToRight(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(1, 0));
        }

        public void MoveCreatureToLeft(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(-1, 0));
        }

        public void MoveCreatureToRightAndToUp(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(1, -creature.Velocity));
            creature.Velocity--;
            if (creature.Velocity <= 0)
                creature.ChangeConditionToFalling();
        }

        public void MoveCreatureToLeftAndToUp(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(-1, -creature.Velocity));
            creature.Velocity--;
            if (creature.Velocity <= 0)
                creature.ChangeConditionToFalling();
        }

        public void MoveCreatureToRightAndToDown(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(1, creature.Velocity / 10));
            creature.Velocity++;
        }

        public void MoveCreatureToLeftAndToDown(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(-1, creature.Velocity / 10));
            creature.Velocity++;
        }

        public void MoveCreatureToDown(IMovingCreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(0, creature.Velocity / 10));
            creature.Velocity++;
        }

        public void CheckCreaturesForFalling()
        {
            foreach (var creature in ListOfCreatures.OfType<IMovingCreature>())
            {
                if (creature.IsFalling() || creature.IsJumping() || !IsThereNothingUnderCreature(creature))
                    continue;
                creature.RecoverVelocity();
                creature.ChangeConditionToFallingDown();
            }
        }

        private bool IsThereNothingUnderCreature(IMovingCreature creature)
        {
            return IsMovementPossible(creature, creature.Location + new Size(0, 1));
        }

        private void MoveCreatureOn(IMovingCreature creature, Point targetLocation)
        {
            if (!IsMovementPossible(creature, targetLocation))
            {
                creature.RecoverVelocity();
                if (creature.IsFalling() || creature.IsJumping())
                    creature.ChangeConditionToStanding();
                return;
            }

            map[creature.Location.X, creature.Location.Y] = null;
            map[targetLocation.X, targetLocation.Y] = creature;
            creature.Location = targetLocation;
        }

        private bool IsMovementPossible(IMovingCreature creature, Point target)
        {
            var topLeftCorner = new Point(
                Math.Min(target.X, creature.Location.X),
                Math.Min(target.Y, creature.Location.Y));
            var bottomRightCorner = new Point(
                Math.Max(target.X, creature.Location.X),
                Math.Max(target.Y, creature.Location.Y));
            return IsPointInBounds(target)
                   && IsPointInBounds(target + creature.Size)
                   && IsMapPieceEmpty(topLeftCorner, bottomRightCorner);
        }

        // Тут стоит временный костыль Map[x, y] != Player
        // В дальнейшем его нужно будет обязательно убрать
        private bool IsMapPieceEmpty(Point topLeftCorner, Point bottomRightCorner)
        {
            for (var x = topLeftCorner.X; x <= bottomRightCorner.X; x++)
            for (var y = topLeftCorner.Y; y <= bottomRightCorner.Y; y++)
                if (map[x, y] != Player && map[x, y] != null)
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
    }
}