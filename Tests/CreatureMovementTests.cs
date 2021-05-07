using System.Drawing;
using System.Linq;
using Model;
using Model.Creatures;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CreatureMovementTests
    {
        private static GameMap map;

        [Test]
        public void CreatureMoveToEverySide()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test1.txt");
            map = new GameMap();
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

            map.MoveCreature(map.Player, Direction.Right, Direction.NoMovement);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.NoMovement);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Right, Direction.Up);
            Assert.AreEqual(expectedCreatureCoordinates[2], map.GetCreatureLocation(map.Player));

            map.Player.RecoverVelocity();
            map.MoveCreature(map.Player, Direction.Left, Direction.Up);
            Assert.AreEqual(expectedCreatureCoordinates[3], map.GetCreatureLocation(map.Player));

            map.Player.RecoverVelocity();
            map.MoveCreature(map.Player, Direction.Right, Direction.Down);
            Assert.AreEqual(expectedCreatureCoordinates[4], map.GetCreatureLocation(map.Player));

            map.Player.RecoverVelocity();
            map.MoveCreature(map.Player, Direction.Left, Direction.Down);
            Assert.AreEqual(expectedCreatureCoordinates[5], map.GetCreatureLocation(map.Player));

            map.Player.RecoverVelocity();
            map.MoveCreature(map.Player, Direction.NoMovement, Direction.Down);
            Assert.AreEqual(expectedCreatureCoordinates[6], map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void CreatureCanNotMoveOffMap()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test2.txt");
            map = new GameMap();
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            map.MoveCreature(map.Player, Direction.Right, Direction.NoMovement);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.NoMovement);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Right, Direction.Up);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.Up);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Right, Direction.Down);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.Down);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.NoMovement, Direction.Down);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [TestCase("test3.txt", TestName = "Creature can't move to right and left")]
        [TestCase("test4.txt", TestName = "Creature can't move to up and down")]
        public void CannotMoveDiagonallyWhenImpossibleMoveInAnyDirection(string level)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel(level);
            map = new GameMap();
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            map.MoveCreature(map.Player, Direction.Right, Direction.Up);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.Up);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Right, Direction.Down);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));

            map.MoveCreature(map.Player, Direction.Left, Direction.Down);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [TestCase("test3.txt", TestName = "There is not empty space under the creatures")]
        [TestCase("test4.txt", TestName = "Empty space under the creatures")]
        public void CheckCreaturesForFallingTestWhenCreaturesAreJumpingOrFalling(string level)
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel(level);
            map = new GameMap();

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, creature.Direction);

            var expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(), expectedMovementConditionsOfCreatures);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, creature.Direction);

            expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(), expectedMovementConditionsOfCreatures);
        }

        [Test]
        public void CreatureShouldFallWhenEmptySpaceUnderItAndWhenNotJumping()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test3.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Dying, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, map.Player.Direction);
            map.CheckCreaturesForFalling();
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);
        }


        [Test]
        public void CreatureShouldNotFallWhenNotEmptySpaceUnderIt()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test4.txt");
            map = new GameMap();

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, map.Player.Direction);
            var expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Dying, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(expectedMovementCondition, map.Player.MovementCondition);
        }


        [Test]
        public void UpdatePlayerLocationOnMap()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test1.txt");
            map = new GameMap();

            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, 0),
                initCreatureLocation,
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(0, -2 *map.Player.Velocity),
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation,
                initCreatureLocation + new Size(0, map.Player.Velocity)
            };

            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Left);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.Player.RecoverVelocity();
            Assert.AreEqual(expectedCreatureCoordinates[2], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, Direction.Left);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.Player.RecoverVelocity();
            Assert.AreEqual(expectedCreatureCoordinates[3], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.Player.RecoverVelocity();
            Assert.AreEqual(expectedCreatureCoordinates[4], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, Direction.Left);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.Player.RecoverVelocity();
            Assert.AreEqual(expectedCreatureCoordinates[5], map.GetCreatureLocation(map.Player));
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Falling, Direction.NoMovement);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.Player.RecoverVelocity();
            Assert.AreEqual(expectedCreatureCoordinates[6], map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void StartsFallingAfterJumpIfPlayerUnderTheCeilingAndStandsAfterLanding()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test7.txt");
            map = new GameMap();
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);
            
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(MovementConditions.Standing, map.Player.MovementCondition);
        }
        
        [Test]
        public void StartsFallingInSomeTimeAfterJump()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test8.txt");
            map = new GameMap();
            
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, Direction.Right);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(MovementConditions.Falling, map.Player.MovementCondition);
        }

        [Test]
        public void PlayerDiesIfHealthEqualsToZero()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test8.txt");
            map = new GameMap();
            
            map.Player.ChangeHealthBy(-100);
            Assert.AreEqual(0, map.Player.Health);
            Assert.AreEqual(MovementConditions.Dying, map.Player.MovementCondition);
        }

        [Test]
        public void PlayerCannotMoveIfSomethingIsNextToHim()
        {
            MapCreator.LoadLevels("LevelsForTests");
            MapCreator.GoToLevel("test9.txt");
            map = new GameMap();

            var initialPosition = map.GetCreatureLocation(map.Player);
            map.MoveCreature(map.Player, Direction.Right, Direction.NoMovement);
            Assert.AreEqual(initialPosition, map.GetCreatureLocation(map.Player));
            
            map.MoveCreature(map.Player, Direction.Left, Direction.NoMovement);
            Assert.AreEqual(initialPosition, map.GetCreatureLocation(map.Player));
            
            map.MoveCreature(map.Player, Direction.NoMovement, Direction.Down);
            Assert.AreEqual(initialPosition, map.GetCreatureLocation(map.Player));
            
            map.MoveCreature(map.Player, Direction.Right, Direction.Up);
            Assert.AreEqual(initialPosition, map.GetCreatureLocation(map.Player));
        }
        
        private static MovementConditions[] GetMovementConditionsOfCreaturesOnMap()
        {
            return map.ListOfCreatures
                .OfType<IMovingCreature>()
                .Select(creature => creature.MovementCondition)
                .ToArray();
        }
    }
}