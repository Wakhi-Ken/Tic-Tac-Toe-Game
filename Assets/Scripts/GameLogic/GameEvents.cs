using System;

namespace GameLogic
{
    /// <summary>
    /// Observer pattern implementation for game events
    /// Decouples game logic from UI
    /// </summary>
    public static class GameEvents
    {
        public static event Action<int> OnMoveMade;
        public static event Action<int> OnGameWon;
        public static event Action OnGameDraw;
        public static event Action OnGameReset;

        public static void MoveMade(int index)
        {
            OnMoveMade?.Invoke(index);
        }

        public static void GameWon(int winner)
        {
            OnGameWon?.Invoke(winner);
        }

        public static void GameDraw()
        {
            OnGameDraw?.Invoke();
        }

        public static void GameReset()
        {
            OnGameReset?.Invoke();
        }
    }
}