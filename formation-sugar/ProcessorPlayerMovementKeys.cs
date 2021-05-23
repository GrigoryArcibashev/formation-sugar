using Model;
using Model.Creatures;

namespace formation_sugar
{
    public static class ProcessorPlayerMovementKeys
    {
        public static void ProcessPlayerMovementKeys(
            Player player,
            bool upIsPressed,
            bool toRightIsPressed,
            bool toLeftIsPressed,
            bool hitIsPressed)
        {
            if (player.IsDead())
                return;

            if (upIsPressed && !player.IsFallingOrJumping())
                MakePlayerJump(player);

            if (toRightIsPressed || toLeftIsPressed)
                MovePlayerToLeftOrRight(player, toRightIsPressed);
            else if (!player.IsFallingOrJumping())
                MakePlayerStand(player);
            else if (player.IsFallingOrJumping() && !(player.Direction is Direction.NoMovement))
                player.ChangeMovementConditionAndDirectionTo(player.MovementCondition, Direction.NoMovement);

            if (hitIsPressed && !player.IsFallingOrJumping())
                MakePlayerAttack(player);
        }

        private static void MakePlayerAttack(Player player)
        {
            player.ChangeMovementConditionAndDirectionTo(
                MovementCondition.Attacking,
                player.Direction is Direction.NoMovement ? Direction.Right : player.Direction);
        }

        private static void MakePlayerStand(Player player)
        {
            player.ChangeMovementConditionAndDirectionTo(
                MovementCondition.Standing,
                player.Direction == Direction.NoMovement ? Direction.Right : player.Direction);
        }

        private static void MovePlayerToLeftOrRight(Player player, bool movingToRight)
        {
            player.ChangeMovementConditionAndDirectionTo(
                player.IsFallingOrJumping() ? player.MovementCondition : MovementCondition.Running,
                movingToRight ? Direction.Right : Direction.Left);
        }

        private static void MakePlayerJump(Player player)
        {
            player.ChangeMovementConditionAndDirectionTo(
                MovementCondition.Jumping,
                player.MovementCondition is MovementCondition.Running ? player.Direction : Direction.NoMovement);
        }
    }
}