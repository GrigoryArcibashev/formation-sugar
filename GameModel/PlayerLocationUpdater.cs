namespace formation_sugar.GameModel
{
    public static class PlayerLocationUpdater
    {
        //private static readonly Physics Physics = new Physics(-10);

        public static void UpdatePlayerLocation(GameMap map)
        {
            switch (map.Player.MovementCondition)
            {
                case MovementConditions.RunningRight:
                    map.MoveCreatureToRight(map.Player);
                    break;

                case MovementConditions.RunningLeft:
                    map.MoveCreatureToLeft(map.Player);
                    break;

                /*case MovementConditions.JumpingRight:
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
                    break;*/
            }
        }
    }
}