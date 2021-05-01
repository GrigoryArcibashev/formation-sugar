namespace formation_sugar.GameModel
{
    public static class PlayerLocationUpdater
    {
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

                case MovementConditions.JumpingRight:
                    map.MoveCreatureUp(map.Player);
                    map.MoveCreatureToRight(map.Player);
                    break;

                case MovementConditions.JumpingLeft:
                    map.MoveCreatureUp(map.Player);
                    map.MoveCreatureToLeft(map.Player);
                    break;

                case MovementConditions.FallingRight:
                    map.MoveCreatureDown(map.Player);
                    map.MoveCreatureToRight(map.Player);
                    break;

                case MovementConditions.FallingLeft:
                    map.MoveCreatureDown(map.Player);
                    map.MoveCreatureToLeft(map.Player);
                    break;
            }
        }
    }
}