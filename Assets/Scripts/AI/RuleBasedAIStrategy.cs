using UnityEngine;

namespace AI
{
    /// <summary>
    /// Rule-based AI strategy for Tic-Tac-Toe
    /// Algorithm:
    /// 1. Check for winning move (AI can win)
    /// 2. Check for blocking move (Opponent can win)
    /// 3. Random move (fallback)
    /// </summary>
    public class RuleBasedAIStrategy : IAIStrategy
    {
        private int[,] winPatterns = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, // Rows
            {0,3,6}, {1,4,7}, {2,5,8}, // Columns
            {0,4,8}, {2,4,6}           // Diagonals
        };

        public int GetAIMove(int[] boardState)
        {
            // Step 1: Check for winning move (AI is player 2)
            int winningMove = FindWinningMove(boardState, 2);
            if (winningMove != -1) return winningMove;

            // Step 2: Check for blocking move (Human is player 1)
            int blockingMove = FindWinningMove(boardState, 1);
            if (blockingMove != -1) return blockingMove;

            // Step 3: Random move fallback
            return GetRandomMove(boardState);
        }

        private int FindWinningMove(int[] boardState, int player)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = winPatterns[i, 0];
                int b = winPatterns[i, 1];
                int c = winPatterns[i, 2];

                // Check if two cells are filled by player and third is empty
                if (boardState[a] == player && boardState[b] == player && boardState[c] == 0)
                    return c;
                if (boardState[a] == player && boardState[c] == player && boardState[b] == 0)
                    return b;
                if (boardState[b] == player && boardState[c] == player && boardState[a] == 0)
                    return a;
            }
            return -1;
        }

        private int GetRandomMove(int[] boardState)
        {
            // Count empty cells
            int emptyCount = 0;
            for (int i = 0; i < boardState.Length; i++)
                if (boardState[i] == 0) emptyCount++;

            // Pick random empty cell
            int randomIndex = Random.Range(0, emptyCount);

            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] == 0)
                {
                    if (randomIndex == 0) return i;
                    randomIndex--;
                }
            }
            return 0;
        }
    }
}