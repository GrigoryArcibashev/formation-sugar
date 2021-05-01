using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace formation_sugar.GameModel
{
    public class GameMap
    {
        private readonly List<string> levels;

        public List<ICreature> ListOfCreatures { get; private set; }
        public ICreature[,] Map { get; private set; }
        public Player Player { get; private set; }
        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);

        public GameMap(int levelNumber)
        {
            levels = new List<string>();
            ListOfCreatures = new List<ICreature>();
            AddLevels();
            CreateMap(levelNumber);
        }

        public void MoveCreatureToRight(ICreature creature)
        {
            MoveCreatureOn(creature, creature.Location + new Size(1, 0));
        }

        public void MoveCreatureToLeft(ICreature creature)
        {
            MoveCreatureOn(creature, creature.Location - new Size(1, 0));
        }

        private void MoveCreatureOn(ICreature creature, Point targetLocation)
        {
            if (!IsMovementPossible(creature, targetLocation))
                return;
            Map[creature.Location.X, creature.Location.Y] = null;
            Map[targetLocation.X, targetLocation.Y] = creature;
            creature.Location = targetLocation;
        }

        private bool IsMovementPossible(ICreature creature, Point target)
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
                if (Map[x, y] != Player && Map[x, y] != null)
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
            var mapInfo = MapCreator.CreateMap(File.ReadAllLines(levels[levelNumber - 1]));
            ListOfCreatures = mapInfo.ListOfCreatures;
            Player = mapInfo.Player;
            Map = mapInfo.Map;
        }

        private void AddLevels()
        {
            var directoryWithLevels = new DirectoryInfo(
                Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName ??
                    throw new InvalidOperationException(), "levels"));
            foreach (var levelName in directoryWithLevels.EnumerateFiles())
                levels.Add(levelName.FullName);
        }
    }
}