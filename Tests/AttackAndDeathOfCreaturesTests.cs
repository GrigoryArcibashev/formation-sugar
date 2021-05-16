using Model;
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
    }
}