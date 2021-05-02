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
                    map.MoveCreatureToRightAndToUp(map.Player);
                    break;

                case MovementConditions.JumpingLeft:
                    map.MoveCreatureToLeftAndToUp(map.Player);
                    break;

                case MovementConditions.FallingRight:
                    map.MoveCreatureToRightAndToDown(map.Player);
                    break;

                case MovementConditions.FallingLeft:
                    map.MoveCreatureToLeftAndToDown(map.Player);
                    break;
                
                case MovementConditions.FallingDown:
                    map.MoveCreatureToDown(map.Player);
                    break;
            }
        }
    }
}