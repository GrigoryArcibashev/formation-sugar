namespace Model
{
    public static class PlayerLocationUpdater
    {
        public static void UpdatePlayerLocation(GameMap map)
        {
            switch (map.Player.MovementCondition)
            {
                case MovementConditions.Running:
                    map.MoveCreature(map.Player, map.Player.Direction);
                    break;

                case MovementConditions.Jumping:
                    if (map.MoveCreature(map.Player, Direction.Up))
                        map.MoveCreature(map.Player, map.Player.Direction);
                    break;

                case MovementConditions.Falling:
                    if (map.MoveCreature(map.Player, Direction.Down))
                        map.MoveCreature(map.Player, map.Player.Direction);
                    break;
            }
        }
    }
}