using Model;

namespace formation_sugar
{
    public static class ProcessorPlayerMovementKeys
    {
        public static void ProcessPlayerMovementKeys(
            GameMap map,
            bool upIsPressed,
            bool toRightIsPressed,
            bool toLeftIsPressed,
            bool hitIsPressed)
        {
            if (map.Player.IsDead())
                return;

            if (upIsPressed && !map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(
                    MovementCondition.Jumping,
                    map.Player.MovementCondition is MovementCondition.Running
                        ? map.Player.Direction
                        : Direction.NoMovement);

            if (toRightIsPressed || toLeftIsPressed)
                map.Player.ChangeMovementConditionAndDirectionTo(
                    map.Player.IsFallingOrJumping() ? map.Player.MovementCondition : MovementCondition.Running,
                    toRightIsPressed ? Direction.Right : Direction.Left);
            else if (map.Player.IsFallingOrJumping() && !(map.Player.Direction is Direction.NoMovement))
                map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.NoMovement);
            else if (!map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(
                    MovementCondition.Standing,
                    map.Player.Direction == Direction.NoMovement ? Direction.Right : map.Player.Direction);

            if (hitIsPressed && !map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(
                    MovementCondition.Attacking,
                    map.Player.Direction is Direction.NoMovement ? Direction.Right : map.Player.Direction);
        }
    }
}