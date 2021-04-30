using System.Drawing;

namespace formation_sugar.GameModel
{
    public static class PlayerLocationUpdater
    {
        private static readonly Physics Physics = new Physics(-10);

        public static void UpdatePlayerLocation(GameMap map, int windowHeight)
        {
            switch (map.Player.MovementCondition)
            {
                case MovementConditions.RunningRight:
                    if (IsMovementPossible(map, new Point(map.Player.Location.X + 1, map.Player.Location.Y)))
                        map.Player.Location = new Point(map.Player.Location.X + 1, map.Player.Location.Y);
                    break;

                case MovementConditions.RunningLeft:

                    if (IsMovementPossible(map, new Point(map.Player.Location.X - 1, map.Player.Location.Y)))
                        map.Player.Location = new Point(map.Player.Location.X - 1, map.Player.Location.Y);
                    break;

                case MovementConditions.JumpingRight:
                    if (map.Player.Velocity < 0) map.Player.MovementCondition = MovementConditions.FallingRight;
                    Physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    map.Player.Location = new Point(map.Player.Location.X + 1, map.Player.Location.Y);
                    break;

                case MovementConditions.JumpingLeft:
                    if (map.Player.Velocity < 0) map.Player.MovementCondition = MovementConditions.FallingLeft;
                    Physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    map.Player.Location = new Point(map.Player.Location.X - 1, map.Player.Location.Y);
                    break;

                case MovementConditions.FallingRight:
                    Physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    map.Player.Location = new Point(map.Player.Location.X + 1, map.Player.Location.Y);
                    break;

                case MovementConditions.FallingLeft:
                    Physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    map.Player.Location = new Point(map.Player.Location.X - 1, map.Player.Location.Y);
                    break;
            }

            IfPlayerOnTheBoard(map, windowHeight);
        }

        private static void IfPlayerOnTheBoard(GameMap map, int height)
        {
            if (map.Player.Location.Y >= height - 40 && map.Player.IsPlayerFalling())
            {
                map.Player.RecoverVelocity();
                map.Player.MovementCondition = map.Player.MovementCondition == MovementConditions.FallingRight
                    ? MovementConditions.StandingRight
                    : MovementConditions.StandingLeft;
            }
        }

        private static bool IsMovementPossible(GameMap map, Point target)
        {
            var x = target.X / 50 + (target.X % 50 > 0 ? 1 : 0);
            var y = target.Y / 50 + (target.Y % 50 > 0 ? 1 : 0);
            return map.Map[x, y] is null;
        }
    }
}