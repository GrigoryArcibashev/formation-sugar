using System.Collections.Generic;
using System.Drawing;
using formation_sugar.GameModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestsPhysics
    {
        [TestCase(0, 0, -10, new[] {0.0}, 0, 0, TestName = "DeltaTime is zero and initial velocity is zero")]
        [TestCase(0, 1, -10, new[] {0.0}, 1, 1, TestName = "DeltaTime is zero")]
        [TestCase(0, 1, 0, new[] {1.0}, 1, 1, TestName = "Gravitational acceleration is zero")]
        [TestCase(0, 4, -8, new[] {0.25, 0.25, 0.25}, 20, -2, TestName = "Highest point of ascent")]
        [TestCase(0, 4, -8, new[] {0.25, 0.25, 0.25, 0.25, 0.25}, 0, -6, TestName = "Full free fall path")]
        [TestCase(
            0,
            4,
            -8,
            new[] {0.25, 0.25, 0.25, 0.25, 0.25, 0.25},
            -36,
            -8,
            TestName = "Continuation of the fall after re-passing the initial coordinate")]
        public void SimpleTests(int initialY, double initialVelocity, double g, IEnumerable<double> dts, int expectedY,
            double expectedVelocity)
        {
            var player = new Player(new Point(0, initialY), new Size(), initialVelocity: initialVelocity);
            var physics = new Physics(g);

            foreach (var dt in dts)
                physics.MoveCreatureByY(player, dt);

            Assert.AreEqual(expectedVelocity, player.Velocity);
            Assert.AreEqual(new Point(0, expectedY), player.Location);
        }
    }
}