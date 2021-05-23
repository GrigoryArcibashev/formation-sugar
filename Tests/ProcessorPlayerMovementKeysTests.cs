using formation_sugar;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ProcessorPlayerMovementKeysTests
    {
        private GameMap map;

        [Test]
        public void KeysAreNotProcessedIfPlayerIsDead()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            map.Player.ChangeHealthBy(map.Player.Health);
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            Assert.AreEqual(true, map.Player.IsDead());

            var currentDirection = map.Player.Direction;
            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                true,
                false,
                false,
                false);

            Assert.AreEqual(true, map.Player.IsDead());
            Assert.AreEqual(currentDirection, map.Player.Direction);
        }

        [TestCase(MovementCondition.Running, Direction.Right)]
        [TestCase(MovementCondition.Running, Direction.Left)]
        [TestCase(MovementCondition.Standing, Direction.Right)]
        [TestCase(MovementCondition.Standing, Direction.Left)]
        public void PlayerAttackIfAttackKeyIsPressedAndHeIsNotJumpingOrFalling(
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            foreach (var (toRightIsPressed, toLeftIsPressed) in new[]
            {
                (false, false),
                (false, true),
                (true, false),
                (true, true)
            })
            {
                map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
                ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                    map.Player,
                    false,
                    toRightIsPressed,
                    toLeftIsPressed,
                    true);

                Assert.AreEqual(true, map.Player.IsAttacking());
                Assert.AreEqual(initialDirection, map.Player.Direction);
            }
        }

        [TestCase(MovementCondition.Running, Direction.Right)]
        [TestCase(MovementCondition.Running, Direction.Left)]
        [TestCase(MovementCondition.Standing, Direction.Right)]
        [TestCase(MovementCondition.Standing, Direction.Left)]
        public void PlayerJumpsIfJumpKeyIsPressedAndIfPlayerDoesNotJumpNow(
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                true,
                false,
                false,
                false);

            Assert.AreEqual(true, map.Player.IsJumping());
            Assert.AreEqual(Direction.NoMovement, map.Player.Direction);
        }

        [TestCase(true, false, MovementCondition.Standing, Direction.Right)]
        [TestCase(false, true, MovementCondition.Standing, Direction.Right)]
        public void PlayerRunIfMoveToSideKeyIsPressed(
            bool toRightIsPressed,
            bool toLeftIsPressed,
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                false,
                toRightIsPressed,
                toLeftIsPressed,
                false);

            Assert.AreEqual(true, map.Player.IsRunning());
            Assert.AreEqual(toRightIsPressed ? Direction.Right : Direction.Left, map.Player.Direction);
        }

        [TestCase(true, false, MovementCondition.Jumping, Direction.Right)]
        [TestCase(true, false, MovementCondition.Jumping, Direction.Left)]
        [TestCase(true, false, MovementCondition.Jumping, Direction.NoMovement)]
        [TestCase(false, true, MovementCondition.Jumping, Direction.Right)]
        [TestCase(false, true, MovementCondition.Jumping, Direction.Left)]
        [TestCase(false, true, MovementCondition.Jumping, Direction.NoMovement)]
        [TestCase(true, false, MovementCondition.Falling, Direction.Right)]
        [TestCase(true, false, MovementCondition.Falling, Direction.Left)]
        [TestCase(true, false, MovementCondition.Falling, Direction.NoMovement)]
        [TestCase(false, true, MovementCondition.Falling, Direction.Right)]
        [TestCase(false, true, MovementCondition.Falling, Direction.Left)]
        [TestCase(false, true, MovementCondition.Falling, Direction.NoMovement)]
        public void PlayerMovesToSideInFlightIfMoveToSideKeyIsPressedAndPlayerIsJumpingOrFalling(
            bool toRightIsPressed,
            bool toLeftIsPressed,
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            foreach (var upIsPressed in new[] {false, true})
            {
                map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
                ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                    map.Player,
                    upIsPressed,
                    toRightIsPressed,
                    toLeftIsPressed,
                    false);

                Assert.AreEqual(true, map.Player.IsFallingOrJumping());
                Assert.AreEqual(toRightIsPressed ? Direction.Right : Direction.Left, map.Player.Direction);
            }
        }

        [Test]
        public void PlayerMovesToRightIfLeftRightKeysArePressedTogether()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                false,
                true,
                true,
                false);

            Assert.AreEqual(Direction.Right, map.Player.Direction);
        }

        [TestCase(MovementCondition.Running, Direction.Right)]
        [TestCase(MovementCondition.Running, Direction.Left)]
        [TestCase(MovementCondition.Standing, Direction.Right)]
        [TestCase(MovementCondition.Standing, Direction.Left)]
        [TestCase(MovementCondition.Attacking, Direction.Right)]
        [TestCase(MovementCondition.Attacking, Direction.Left)]
        public void PlayerStandsIfAllKeysAreNotPressedAndIfHeIsNotJumpingOrFalling(
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                false,
                false,
                false,
                false);

            Assert.AreEqual(true, map.Player.IsStanding());
            Assert.AreEqual(initialDirection, map.Player.Direction);
        }

        [TestCase(MovementCondition.Jumping, Direction.Right)]
        [TestCase(MovementCondition.Jumping, Direction.Left)]
        [TestCase(MovementCondition.Jumping, Direction.NoMovement)]
        [TestCase(MovementCondition.Falling, Direction.Right)]
        [TestCase(MovementCondition.Falling, Direction.Left)]
        [TestCase(MovementCondition.Falling, Direction.NoMovement)]
        public void PlayerWillNotBeAbleToStandIfAllKeysAreNotPressedAndIfHeIsJumpingOrFallingOrAttacking(
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
            ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                map.Player,
                false,
                false,
                false,
                false);

            Assert.AreEqual(false, map.Player.IsStanding());
            Assert.AreEqual(Direction.NoMovement, map.Player.Direction);
        }


        [TestCase(MovementCondition.Jumping, Direction.Right)]
        [TestCase(MovementCondition.Jumping, Direction.Left)]
        [TestCase(MovementCondition.Jumping, Direction.NoMovement)]
        [TestCase(MovementCondition.Falling, Direction.Right)]
        [TestCase(MovementCondition.Falling, Direction.Left)]
        [TestCase(MovementCondition.Falling, Direction.NoMovement)]
        public void PlayerMustFallOrJumpWithoutMovingHorizontallyIfToRightIfLeftRightKeysAreNotPressed(
            MovementCondition initialMovementCondition,
            Direction initialDirection)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test19.txt");
            map = new GameMap();

            foreach (var upIsPressed in new[] {false, true})
            {
                map.Player.ChangeMovementConditionAndDirectionTo(initialMovementCondition, initialDirection);
                ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                    map.Player,
                    upIsPressed,
                    false,
                    false,
                    false);

                Assert.AreEqual(initialMovementCondition, map.Player.MovementCondition);
                Assert.AreEqual(Direction.NoMovement, map.Player.Direction);
            }
        }
    }
}