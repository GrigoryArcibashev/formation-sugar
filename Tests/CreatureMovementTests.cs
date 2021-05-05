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
            
            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeft(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[1], map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[2], map.GetCreatureLocation(map.Player));
            
            map.Player.RecoverVelocity();
            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[3], map.GetCreatureLocation(map.Player));
            
            map.Player.RecoverVelocity();
            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[4], map.GetCreatureLocation(map.Player));
            
            map.Player.RecoverVelocity();
            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[5], map.GetCreatureLocation(map.Player));
            
            map.Player.RecoverVelocity();
            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureCoordinates[6], map.GetCreatureLocation(map.Player));
        }

        [Test]
        public void CreatureCanNotMoveOffMap()
        {
            map = new GameMap("test2.txt");
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);
            
            map.MoveCreatureToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeft(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToRight(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [TestCase("test3.txt", TestName = "Creature can't move to right and left")]
        [TestCase("test4.txt", TestName = "Creature can't move to up and down")]
        public void CannotMoveDiagonallyWhenImpossibleMoveInAnyDirection(string level)
        {
            map = new GameMap(level);
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);
            
            map.MoveCreatureToRightAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeftAndToUp(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToRightAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            
            map.MoveCreatureToLeftAndToDown(map.Player);
            Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
        }

        [TestCase("test3.txt", TestName = "There is not empty space under the creatures")]
        [TestCase("test4.txt", TestName = "Empty space under the creatures")]
        public void CheckCreaturesForFallingTestWhenCreaturesAreJumpingOrFalling(string level)
        {
            map = new GameMap(level);

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
            map = new GameMap("test3.txt");
            
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
            map = new GameMap("test4.txt");
            
            var expectedMovementCondition = MovementConditions.Default;
            map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Attacking, map.Player.Direction);
            expectedMovementCondition = GetMovementConditionsOfCreaturesOnMap().FirstOrDefault();
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
            map = new GameMap("test1.txt");

            var initCreatureLocation = map.GetCreatureLocation(map.Player);
            var expectedCreatureCoordinates = new[]
            {
                initCreatureLocation + new Size(1, -map.Player.Velocity),
                initCreatureLocation + new Size(2, 0),
                initCreatureLocation + new Size(2, map.Player.Velocity),
                initCreatureLocation + new Size(3, map.Player.Velocity)
            };
            
            
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            Assert.AreEqual(expectedCreatureCoordinates[0], map.GetCreatureLocation(map.Player));
        }

        /*
        [Test]
        public void UpdateCreatureMovementCondition()
        {
            map = new GameMap("test6.txt");
            var expectedMovementConditionsForRightSide = new []
            {
                MovementConditions.Jumping
                MovementConditions.Falling
                MovementConditions.Falling
                MovementConditions.Running,
                MovementConditions.StandingRight,
                MovementConditions.SittingRight,
                MovementConditions.AttackingRight,
                MovementConditions.DieRight
            };
            
            for (var i = 0; i < infoAboutMethodsOfTypeChangeCondition.Length; i++)
            {
                foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                {
                    infoAboutMethodsOfTypeChangeCondition[i].Invoke(
                        creature,
                        infoAboutMethodsOfTypeChangeCondition[i].Name == "ChangeConditionToRun"
                            ? new object[] {Direction.Right}
                            : new object[] { });
                    
                    var conditions = GetMovementConditionsOfCreaturesOnMap();
                    conditions = conditions.Distinct().ToArray();

                    Assert.AreEqual(1, conditions.Length);
                    Assert.AreEqual(expectedMovementConditionsForRightSide[i], creature.MovementCondition);
                }
            }
        }
        */

        private static MovementConditions[] GetMovementConditionsOfCreaturesOnMap()
        {
            return map.ListOfCreatures
                .OfType<IMovingCreature>()
                .Select(creature => creature.MovementCondition)
                .ToArray();
        }
    }
}