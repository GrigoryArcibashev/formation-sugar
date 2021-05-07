namespace Model
{
    public static class PlayerLocationUpdater
    {
        public static void UpdatePlayerLocation(GameMap map)
        {
            switch (map.Player.MovementCondition, map.Player.Direction)
            {
                case (MovementConditions.Running, Direction.Right):
                    map.MoveCreature(map.Player, Direction.Right, Direction.NoMovement);
                    break;

                case (MovementConditions.Running, Direction.Left):
                    map.MoveCreature(map.Player, Direction.Left, Direction.NoMovement);
                    break;

                case (MovementConditions.Jumping, Direction.Right):
                    map.MoveCreature(map.Player, Direction.Right, Direction.Up);
                    break;

                case (MovementConditions.Jumping, Direction.Left):
                    map.MoveCreature(map.Player, Direction.Left, Direction.Up);
                    break;

                case (MovementConditions.Falling, Direction.Right):
                    map.MoveCreature(map.Player, Direction.Right, Direction.Down);
                    break;

                case (MovementConditions.Falling, Direction.Left):
                    map.MoveCreature(map.Player, Direction.Left, Direction.Down);
                    break;

                case (MovementConditions.Falling, Direction.NoMovement):
                    map.MoveCreature(map.Player, Direction.NoMovement, Direction.Down);
                    break;
            }
        }
    }
}