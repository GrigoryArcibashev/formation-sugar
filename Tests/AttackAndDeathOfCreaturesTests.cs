using System.Linq;
using Model;
using Model.Creatures;
using Model.Creatures.CreatureInterfaces;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class AttackAndDeathOfCreaturesTests
    {
        private static GameMap map;

        [Test]
        public void PlayerDiesIfHealthEqualsToZero()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test8.txt");
            map = new GameMap();

            map.Player.ChangeHealthBy(100);
            Assert.AreEqual(0, map.Player.Health);
            Assert.AreEqual(MovementConditions.Dying, map.Player.MovementCondition);
        }

        [Test]
        public void PlayerCannotMoveThroughEnemyAndChests()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test10.txt");
            map = new GameMap();

            var initialPlayerLocation = map.GetCreatureLocation(map.Player);
            map.MoveCreature(map.Player, Direction.Right);
            map.MoveCreature(map.Player, Direction.Left);
            map.MoveCreature(map.Player, Direction.Up);
            map.MoveCreature(map.Player, Direction.Down);
            Assert.AreEqual(initialPlayerLocation, map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void PlayerDiesFromTheEnemy()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test10.txt");
            map = new GameMap();

            for (var i = 0; i < 1000; i++)
            {
                map.MakeEnemiesAttackingOrRunning();
                CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            }

            Assert.AreEqual(MovementConditions.Dying, map.Player.MovementCondition);
        }

        [Test]
        public void EnemyChasesThePlayerIfDistanceIsShort()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test11.txt");
            map = new GameMap();
            map.MakeEnemiesAttackingOrRunning();
            var enemy = map.ListOfCreatures.OfType<IEnemy>().ToArray()[0];
            Assert.AreEqual(MovementConditions.Running, enemy.MovementCondition);
            Assert.AreEqual(Direction.Left, enemy.Direction);
        }

        [Test]
        public void EnemyNotChasesThePlayerIfDistanceIsLong()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test12.txt");
            map = new GameMap();
            map.MakeEnemiesAttackingOrRunning();
            var enemy = map.ListOfCreatures.OfType<IEnemy>().ToArray()[0];
            Assert.AreEqual(MovementConditions.Standing, enemy.MovementCondition);
        }

        [TestCase("test13.txt")]
        [TestCase("test14.txt", TestName = "EnemyOnThePlayer")]
        [TestCase("test15.txt", TestName = "PlayerOnTheEnemy")]
        public void EnemyDiesWithTwoBlowsFromThePlayer(string levelName)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel(levelName);
            map = new GameMap();

            var enemy = map.ListOfCreatures.OfType<IEnemy>().ToArray()[0];
            for (var i = 0; i < 1000; i++)
            {
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, Direction.Right);
                CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            }

            Assert.AreEqual(MovementConditions.Dying, enemy.MovementCondition);
        }

        [Test]
        public void PlayerDoesntKillTheEnemyIfHeIsTurnedAwayFromIt()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test13.txt");
            map = new GameMap();

            var enemy = map.ListOfCreatures.OfType<IEnemy>().ToArray()[0];
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, Direction.Left);
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            Assert.AreNotEqual(MovementConditions.Dying, enemy.MovementCondition);
        }

        [Test]
        public void EnemyDeletesFromTheMapAfterTheDeath()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test13.txt");
            map = new GameMap();

            var enemy = (IMovingCreature) map[1, 4];
            enemy.ChangeMovementConditionAndDirectionTo(MovementConditions.Dying, enemy.Direction);
            var chest = (ICreatureWithHealth) map[1, 3];
            chest.ChangeHealthBy(int.MaxValue);

            map.RemoveCreaturesFromMapIfTheyAreDead();

            Assert.AreEqual(false, map.ListOfCreatures.Contains(enemy));
            Assert.AreEqual(null, map[1, 4]);
            Assert.AreEqual(false, map.ListOfCreatures.Contains(chest));
            Assert.AreEqual(null, map[1, 3]);
        }

        [Test]
        public void PlayerIsOnTheEnemyAndFallsAfterHeKillsHim()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test16.txt");
            map = new GameMap();
            var expectedPlayerLocation = map.GetCreatureLocation(map.Player);
            expectedPlayerLocation.Y++;
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, Direction.Right);
            for (var i = 0; i < 1000; i++)
            {
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, Direction.Right);
                CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            }
            
            map.RemoveCreaturesFromMapIfTheyAreDead();
            map.CheckCreaturesForFalling();
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            Assert.AreEqual(expectedPlayerLocation, map.GetCreatureLocation(map.Player));
        }
    }
}