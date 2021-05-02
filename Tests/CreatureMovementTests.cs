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
            var initCreatureLocation = map.Player.Location;
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, 0),
                initCreatureLocation + new Size(-1, 0)
            };

            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);
            map.Player.Location = initCreatureLocation;

            map.MoveCreatureToLeft(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);
        }

        [Test]
        public void CreatureMoveUp()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.Player.Location;
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(-1, -map.Player.Velocity + 1)
            };

            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);
            map.Player.Location = initCreatureLocation;

            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);
        }

        [Test]
        public void CreatureMoveDown()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.Player.Location;
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, map.Player.Velocity / 10),
                initCreatureLocation + new Size(-1, (map.Player.Velocity + 1) / 10),
                initCreatureLocation + new Size(0, (map.Player.Velocity + 2) / 10)
            };

            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[0].X, expectedCreatureCoordinates[0].Y], map.Player);
            map.Player.Location = initCreatureLocation;

            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[1].X, expectedCreatureCoordinates[1].Y], map.Player);
            map.Player.Location = initCreatureLocation;

            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[2], map.Player.Location);
            Assert.AreEqual(map[expectedCreatureCoordinates[2].X, expectedCreatureCoordinates[2].Y], map.Player);
        }

        [Test]
        public void CreatureCanNotMoveOffMap()
        {
            map = new GameMap("test2.txt");
            var expectedCreatureLocation = map.Player.Location;

            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToLeft(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.Player.Location);
        }

        [Test]
        public void ImpossibilityOfDiagonalMovementWhenItIsImpossibleMoveInOneOfDirections()
        {
            foreach (var testName in new[] {"test3.txt", "test4.txt"})
            {
                map = new GameMap(testName);
                var expectedCreatureLocation = map.Player.Location;
                
                map.MoveCreatureToRightAndToUp(map.Player);
                Assert.AreEqual(expectedCreatureLocation, map.Player.Location);

                map.MoveCreatureToLeftAndToDown(map.Player);
                Assert.AreEqual(expectedCreatureLocation, map.Player.Location);
            }
        }

        private static void CheckCreaturesForFalling()
        {
            map.CheckCreaturesForFalling();
        }

        private static void UpdatePlayerLocationOnMap()
        {
            PlayerLocationUpdater.UpdatePlayerLocation(map);
        }
    }
}