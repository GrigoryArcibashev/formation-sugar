using Model;

namespace formation_sugar
{
    public static class GameStatusChecker
    {
        public static bool CheckGameStatus(GameMap map, bool resetLevel = false, bool nextLevel = false)
        {
            if (resetLevel)
            {
                MapCreator.ResetLevel();
                map.ResetScoresForCurrentGame();
            }
            else if (!nextLevel)
                return false;

            map.LoadNextMap(map.TotalScore);
            return true;
        }

        public static bool GameWon(GameMap map)
        {
            return map.Finish.MovementCondition is MovementCondition.Dying;
        }

        public static bool GameOver(GameMap map)
        {
            return map.Player.MovementCondition is MovementCondition.Dying;
        }
    }
}