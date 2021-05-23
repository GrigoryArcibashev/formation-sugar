using formation_sugar;
using Model;
using Model.Creatures;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ScoringTests
    {
        private static GameMap map;

        [Test]
        public void ScoringAfterOpeningChest()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test17.txt");
            map = new GameMap();

            var chest = (Chest) map[1, 0];
            Assert.AreEqual(0, map.TotalScore);

            for (var i = 0; i < 100; i++)
                map.Attack(map.Player);

            Assert.AreEqual(chest.Score, map.TotalScore);
        }

        [Test]
        public void ScoringAfterKillingEnemy()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test18.txt");
            map = new GameMap();

            var enemy = (Enemy) map[1, 0];
            for (var i = 0; i < 100; i++)
                map.Attack(map.Player);

            Assert.AreEqual(enemy.ScoreForKilling, map.TotalScore);
        }

        [Test]
        public void ScoresAreDeductedIfLevelWasRestarted()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test18.txt");
            map = new GameMap();

            var enemy = (Enemy) map[1, 0];
            for (var i = 0; i < 100; i++)
                map.Attack(map.Player);

            Assert.AreEqual(enemy.ScoreForKilling, map.TotalScore);

            GameStatusChecker.CheckGameStatus(map, resetLevel: true);
            Assert.AreEqual(0, map.TotalScore);
        }

        [Test]
        public void ScoresAreSavedIfNextLevelWasLoaded()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test18.txt");
            map = new GameMap();

            var enemy = (Enemy) map[1, 0];
            for (var i = 0; i < 100; i++)
                map.Attack(map.Player);

            var currentScore = map.TotalScore;
            Assert.AreEqual(enemy.ScoreForKilling, currentScore);

            GameStatusChecker.CheckGameStatus(map, nextLevel: true);
            Assert.AreEqual(currentScore, map.TotalScore);
        }
    }
}