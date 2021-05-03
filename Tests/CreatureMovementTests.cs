using System.Drawing;
using System.Linq;
using System.Reflection;
using formation_sugar.GameModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CreatureMovementTests
    {
        private static GameMap map;

        private static readonly MethodInfo[] infosAboutMethodsOfTypeMoveCreature = typeof(GameMap)
            .GetMethods()
            .Where(methodInfo => methodInfo
                .GetCustomAttributes<MoveCreatureAttribute>()
                .Any())
            .ToArray();

        private static readonly MethodInfo[] infosAboutMethodsOfTypeChangeCondition = typeof(IMovingCreature)
            .GetMethods()
            .Where(methodInfo => methodInfo
                .GetCustomAttributes<ChangeConditionAttribute>()
                .Any())
            .ToArray();

        [Test]
        public void CreatureMoveToLeftAndRight()
        {
            map = new GameMap("test1.txt");
            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, 0),
                initCreatureLocation,
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(0, -2 * map.Player.Velocity),
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(0, 0),
                initCreatureLocation + new Size(0, map.Player.Velocity)
            };
            for (var i = 0; i < infosAboutMethodsOfTypeMoveCreature.Length; i++)
            {
                map.Player.RecoverVelocity();
                infosAboutMethodsOfTypeMoveCreature[i].Invoke(map, new object[] {map.Player});
                Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
                Assert.AreEqual(map[expectedCreatureCoordinates[i].X, expectedCreatureCoordinates[i].Y], map.Player);
            }
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
            map.Player.RecoverVelocity();
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, map.Player.Velocity),
                initCreatureLocation + new Size(0, 2 * map.Player.Velocity + 1),
                initCreatureLocation + new Size(0, 3 * map.Player.Velocity + 1 + 2)
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

            foreach (var methodInfo in infosAboutMethodsOfTypeMoveCreature)
            {
                methodInfo.Invoke(map, new object[] {map.Player});
                Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            }
        }

        [TestCase("test3.txt", TestName = "Creature can't move to right and down")]
        [TestCase("test4.txt", TestName = "Creature can't move to left and up")]
        public void ImpossibilityOfDiagonalMovementWhenItIsImpossibleMoveInOneOfDirections(string level)
        {
            map = new GameMap(level);
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [TestCase("test3.txt", TestName = "Creatures can't fall")]
        [TestCase("test4.txt", TestName = "Creatures must fall")]
        public void CheckCreaturesForFallingTestWhenCreaturesAreJumpingOrFalling(string level)
        {
            map = new GameMap(level);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToJumping();
            var expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap(map);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(map), expectedMovementConditionsOfCreatures);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToFalling();
            expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap(map);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(map), expectedMovementConditionsOfCreatures);
        }

        [TestCase("test3.txt", TestName = "Creatures can't fall")]
        [TestCase("test4.txt", TestName = "Creatures must fall")]
        public void CheckCreaturesForFallingTestWhenCreaturesAreNotJumpingOrFalling(string level)
        {
            map = new GameMap(level);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToJumping();
            var expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap(map);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(map), expectedMovementConditionsOfCreatures);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToFalling();
            expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap(map);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(map), expectedMovementConditionsOfCreatures);
        }


        /*[Test]
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
        }*/
        private static MovementConditions[] GetMovementConditionsOfCreaturesOnMap(GameMap map)
        {
            return map.ListOfCreatures
                .OfType<IMovingCreature>()
                .Select(creature => creature.MovementCondition)
                .ToArray();
        }
    }
}