using System;

namespace GameLogic
{
    /// <summary>
    /// Pure logic class - No Unity dependencies
    /// Manages the game board state
    /// </summary>
    public class BoardState
    {
        private int[] board = new int[9];

        public int[] GetBoard()
        {
            return (int[])board.Clone();
        }

        public bool IsCellEmpty(int index)
        {
            return board[index] == 0;
        }

        public void SetCell(int index, int player)
        {
            board[index] = player;
        }

        public int GetCell(int index)
        {
            return board[index];
        }

        public void Reset()
        {
            Array.Clear(board, 0, board.Length);
        }
    }
}