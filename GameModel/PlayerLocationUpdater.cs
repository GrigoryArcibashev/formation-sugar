namespace Model
{
    public static class PlayerLocationUpdater
    {
        public static void UpdatePlayerLocation(GameMap map)
        {
            switch (map.Player.MovementCondition, map.Player.Direction)
            {
                case (MovementConditions.Running, Direction.Right):
                    map.MoveCreatureToRight(map.Player);
                    break;

                case (MovementConditions.Running, Direction.Left):
                    map.MoveCreatureToLeft(map.Player);
                    break;

                case (MovementConditions.Jumping, Direction.Right):
                    map.MoveCreatureToRightAndToUp(map.Player);
                    break;

                case (MovementConditions.Jumping, Direction.Left):
                    map.MoveCreatureToLeftAndToUp(map.Player);
                    break;

                case (MovementConditions.Falling, Direction.Right):
                    map.MoveCreatureToRightAndToDown(map.Player);
                    break;

                case (MovementConditions.Falling, Direction.Left):
                    map.MoveCreatureToLeftAndToDown(map.Player);
                    break;
                
                case (MovementConditions.Falling, Direction.Front):
                    map.MoveCreatureToDown(map.Player);
                    break;
            }
        }
    }
}