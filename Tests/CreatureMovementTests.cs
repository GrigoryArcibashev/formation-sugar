using System.Drawing;
using System.Linq;
using System.Reflection;
using Model;
using Model.Creatures;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CreatureMovementTests
    {
        private static GameMap map;

        private static readonly MethodInfo[] infoAboutMethodsOfTypeMoveCreature = typeof(GameMap)
            .GetMethods()
            .Where(methodInfo => methodInfo
                .GetCustomAttributes<MoveCreatureAttribute>()
                .Any())
            .ToArray();

        private static readonly MethodInfo[] infoAboutMethodsOfTypeChangeCondition = typeof(IMovingCreature)
            .GetMethods()
            .Where(methodInfo => methodInfo
                .GetCustomAttributes<ChangeConditionAttribute>()
                .Any())
            .ToArray();

        private static readonly MethodInfo[] infoAboutMethodsOfTypeMoveCreatureDiagonally =
            infoAboutMethodsOfTypeMoveCreature
                .Where(methodInfo => methodInfo
                    .GetCustomAttributes<MoveCreatureDiagonallyAttribute>()
                    .Any())
                .ToArray();

        private static readonly MethodInfo[] infoAboutMethodsOfTypeChangeConditionThatAffectsMovement =
            infoAboutMethodsOfTypeChangeCondition
                .Where(methodInfo => methodInfo.GetCustomAttributes<ChangeConditionThatAffectsMovementAttribute>().Any())
                .ToArray();

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

            for (var i = 0; i < infoAboutMethodsOfTypeMoveCreature.Length; i++)
            {
                map.Player.RecoverVelocity();
                infoAboutMethodsOfTypeMoveCreature[i].Invoke(map, new object[] {map.Player});
                Assert.AreEqual(expectedCreatureCoordinates[i], map.GetCreatureLocation(map.Player));
                Assert.AreEqual(map[expectedCreatureCoordinates[i].X, expectedCreatureCoordinates[i].Y], map.Player);
            }
        }

        [Test]
        public void CreatureCanNotMoveOffMap()
        {
            map = new GameMap("test2.txt");
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            foreach (var methodInfo in infoAboutMethodsOfTypeMoveCreature)
            {
                methodInfo.Invoke(map, new object[] {map.Player});
                Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            }
        }

        [TestCase("test3.txt", TestName = "Creature can't move to right and left")]
        [TestCase("test4.txt", TestName = "Creature can't move to up and down")]
        public void CannotMoveDiagonallyWhenImpossibleMoveInAnyDirection(string level)
        {
            map = new GameMap(level);
            var expectedCreatureLocation = map.GetCreatureLocation(map.Player);

            foreach (var method in infoAboutMethodsOfTypeMoveCreatureDiagonally)
            {
                method.Invoke(map, new object[] {map.Player});
                Assert.AreEqual(expectedCreatureLocation, map.GetCreatureLocation(map.Player));
            }
        }

        [TestCase("test3.txt", TestName = "There is not empty space under the creatures")]
        [TestCase("test4.txt", TestName = "Empty space under the creatures")]
        public void CheckCreaturesForFallingTestWhenCreaturesAreJumpingOrFalling(string level)
        {
            map = new GameMap(level);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToJumping();
            var expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(), expectedMovementConditionsOfCreatures);

            foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                creature.ChangeConditionToFalling();
            expectedMovementConditionsOfCreatures = GetMovementConditionsOfCreaturesOnMap();
            map.CheckCreaturesForFalling();
            Assert.AreEqual(GetMovementConditionsOfCreaturesOnMap(), expectedMovementConditionsOfCreatures);
        }

        [Test]
        public void CreatureShouldFallWhenEmptySpaceUnderIt()
        {
            map = new GameMap("test3.txt");
            
            foreach (var method in infoAboutMethodsOfTypeChangeCondition.Skip(3))
            {
                foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                {
                    method.Invoke(
                        creature,
                        method.Name == "ChangeConditionToRun"
                            ? new object[] {Direction.Right}
                            : new object[] { });
                }
                
                map.CheckCreaturesForFalling();
                
                foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                {
                    Assert.AreEqual(MovementConditions.FallingDown, creature.MovementCondition);
                }
            }
        }
        
        [Test]
        public void CreatureShouldNotFallWhenNotEmptySpaceUnderIt()
        {
            map = new GameMap("test4.txt");

            foreach (var method in infoAboutMethodsOfTypeChangeCondition.Skip(3))
            {
                foreach (var creature in map.ListOfCreatures.OfType<IMovingCreature>())
                {
                    method.Invoke(
                        creature,
                        method.Name == "ChangeConditionToRun"
                            ? new object[] {Direction.Right}
                            : new object[] { });
                }

                var expectedCondition = GetMovementConditionsOfCreaturesOnMap();
                map.CheckCreaturesForFalling();
                Assert.AreEqual(expectedCondition, GetMovementConditionsOfCreaturesOnMap());
            }
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

            for (var i = 0; i < infoAboutMethodsOfTypeChangeConditionThatAffectsMovement.Length; i++)
            {
                map.Player.RecoverVelocity();
                infoAboutMethodsOfTypeChangeConditionThatAffectsMovement[i].Invoke(
                    map.Player,
                    infoAboutMethodsOfTypeChangeConditionThatAffectsMovement[i].Name == "ChangeConditionToRun"
                        ? new object[] {Direction.Right}
                        : new object[] { });

                PlayerLocationUpdater.UpdatePlayerLocation(map);
                Assert.AreEqual(expectedCreatureCoordinates[i], map.GetCreatureLocation(map.Player));
            }
        }

        [Test]
        public void UpdateCreatureMovementCondition()
        {
            map = new GameMap("test6.txt");
            var expectedMovementConditionsForRightSide = new []
            {
                MovementConditions.JumpingRight,
                MovementConditions.FallingRight,
                MovementConditions.FallingDown,
                MovementConditions.RunningRight,
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

        private static MovementConditions[] GetMovementConditionsOfCreaturesOnMap()
        {
            return map.ListOfCreatures
                .OfType<IMovingCreature>()
                .Select(creature => creature.MovementCondition)
                .ToArray();
        }
    }
}