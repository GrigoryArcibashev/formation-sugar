using formation_sugar.GameModel;

using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MovingPlayerTests
    {
        [Test]
        public void Method1()
        {
            var map1 = new GameMap(1);
            map1.MoveCreatureToRight(map1.Player);
            var map = new GameMap(1);
            var point = 
            map.Map[1, 2]
            Assert.AreEqual(map);
        }
    }
}