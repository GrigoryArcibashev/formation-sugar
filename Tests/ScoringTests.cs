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
            Assert.AreEqual(0, map.Score);

            for (var i = 0; i < 100; i++)
                map.Attack(map.Player);
            Assert.AreEqual(chest.Score, map.Score);
        }
    }
}