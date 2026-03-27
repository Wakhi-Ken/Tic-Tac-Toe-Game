using System;

namespace GameLogic
{
    /// <summary>
    /// Pure logic class - No Unity dependencies
    /// Handles win and draw detection
    /// </summary>
    public class WinChecker
    {
        private int[,] winPatterns = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, // Rows
            {0,3,6}, {1,4,7}, {2,5,8}, // Columns
            {0,4,8}, {2,4,6}           // Diagonals
        };

        /// <summary>
        /// Checks if there's a winner
        /// Returns: 1 = Player X, 2 = Player O, 0 = No winner
        /// </summary>
        public int CheckWinner(int[] board)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = winPatterns[i, 0];
                int b = winPatterns[i, 1];
                int c = winPatterns[i, 2];

                if (board[a] != 0 && board[a] == board[b] && board[b] == board[c])
                    return board[a];
            }
            return 0;
        }

        /// <summary>
        /// Returns the winning pattern index
        /// Returns -1 if no winner
        /// </summary>
        public int GetWinningPattern(int[] board)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = winPatterns[i, 0];
                int b = winPatterns[i, 1];
                int c = winPatterns[i, 2];

                if (board[a] != 0 && board[a] == board[b] && board[b] == board[c])
                    return i;
            }
            return -1;
        }

        public bool IsBoardFull(int[] board)
        {
            foreach (int cell in board)
                if (cell == 0) return false;
            return true;
        }
    }
}