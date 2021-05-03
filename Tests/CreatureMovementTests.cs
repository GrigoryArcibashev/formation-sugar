using System;
using System.Drawing;
using formation_sugar.GameModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CreatureMovementTests
    {
        private static GameMap map;

        [Test]
        public void CreatureMoveToLeftAndRight()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, 0),
                initCreatureLocation
            };

            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);
            
            map.MoveCreatureToLeft(map.Player);
            
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);
        }

        [Test]
        public void CreatureMoveUp()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(0, -2 * map.Player.Velocity + 1)
            };

            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);

            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);
        }

        [Test]
        public void CreatureMoveDown()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, map.Player.Velocity / 10),
                initCreatureLocation + new Size(0, (map.Player.Velocity + 1) / 10 + map.Player.Velocity / 10),
                initCreatureLocation + new Size(0, (map.Player.Velocity + 2) / 10 + (map.Player.Velocity + 1) / 10 + map.Player.Velocity / 10)
            };

            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);

            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);

            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[2], map.GetCreatureLocation(map.Player));
            Assert.AreEqual(map[expectedCreatureCoordinates[2].X, expectedCreatureCoordinates[2].Y], map.Player);
        }

        [Test]
        public void CreatureCanNotMoveOffMap()
        {
            map = new GameMap("test2.txt");
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToLeft(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void ImpossibilityOfDiagonalMovementWhenItIsImpossibleMoveInOneOfDirections()
        {
            foreach (var testName in new[] {"test3.txt", "test4.txt"})
            {
                map = new GameMap(testName);
                var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

                map.MoveCreatureToRightAndToUp(map.Player);
                Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

                map.MoveCreatureToLeftAndToDown(map.Player);
                Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            }
        }

        [Test]
        public void CheckCreaturesForFalling()
        {
            map = new GameMap("test4.txt");
            map.Player.ChangeConditionToFallingDown();
            var initialPlayerLocation = map.GetCreatureLocation(map.Player);

            while (initialPlayerLocation == map.GetCreatureLocation(map.Player))
            {
                PlayerLocationUpdater.UpdatePlayerLocation(map);
            }
            
            Assert.AreEqual(new Point(initialPlayerLocation.X, initialPlayerLocation.Y + 1), map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void UpdatePlayerLocationOnMap()
        {
            map = new GameMap("test1.txt");
            
            var initialPlayerLocation = map.GetCreatureLocation(map.Player);
            map.Player.ChangeConditionToRun(Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            
            Assert.AreEqual(new Point(initialPlayerLocation.X + 1, initialPlayerLocation.Y), map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeConditionToRun(Direction.Left);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            
            Assert.AreEqual(initialPlayerLocation, map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeConditionToFallingDown();

            while (initialPlayerLocation.Y + 1 != map.GetCreatureLocation(map.Player).Y)
            {
                PlayerLocationUpdater.UpdatePlayerLocation(map);
            }
            
            Assert.AreEqual(new Point(initialPlayerLocation.X, initialPlayerLocation.Y + 1), map.GetCreatureLocation(map.Player));
        }
        
        
    }
}