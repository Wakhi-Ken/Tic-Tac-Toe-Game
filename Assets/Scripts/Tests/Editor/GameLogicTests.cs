using NUnit.Framework;
using GameLogic;
using AI;

public class GameLogicTests
{
    [Test]
    public void Test_WinChecker_HorizontalWin()
    {
        int[] board = new int[9];
        board[0] = 1; board[1] = 1; board[2] = 1;

        WinChecker checker = new WinChecker();
        int winner = checker.CheckWinner(board);

        Assert.AreEqual(1, winner);
    }

    [Test]
    public void Test_WinChecker_VerticalWin()
    {
        int[] board = new int[9];
        board[0] = 2; board[3] = 2; board[6] = 2;

        WinChecker checker = new WinChecker();
        int winner = checker.CheckWinner(board);

        Assert.AreEqual(2, winner);
    }

    [Test]
    public void Test_WinChecker_DiagonalWin()
    {
        int[] board = new int[9];
        board[0] = 1; board[4] = 1; board[8] = 1;

        WinChecker checker = new WinChecker();
        int winner = checker.CheckWinner(board);

        Assert.AreEqual(1, winner);
    }

    [Test]
    public void Test_WinChecker_NoWinner()
    {
        int[] board = new int[9];
        board[0] = 1; board[1] = 2; board[2] = 1;
        board[3] = 2; board[4] = 1; board[5] = 2;
        board[6] = 2; board[7] = 1; board[8] = 2;

        WinChecker checker = new WinChecker();
        int winner = checker.CheckWinner(board);

        Assert.AreEqual(0, winner);
    }

    [Test]
    public void Test_BoardState_IsFull()
    {
        BoardState boardState = new BoardState();
        for (int i = 0; i < 9; i++)
            boardState.SetCell(i, 1);

        WinChecker checker = new WinChecker();
        bool isFull = checker.IsBoardFull(boardState.GetBoard());

        Assert.IsTrue(isFull);
    }

    [Test]
    public void Test_BoardState_NotFull()
    {
        BoardState boardState = new BoardState();
        boardState.SetCell(0, 1);
        boardState.SetCell(1, 1);

        WinChecker checker = new WinChecker();
        bool isFull = checker.IsBoardFull(boardState.GetBoard());

        Assert.IsFalse(isFull);
    }

    [Test]
    public void Test_AI_BlocksWinningMove()
    {
        // Human (player 1) has two in a row at positions 0 and 1
        int[] board = new int[9];
        board[0] = 1; board[1] = 1; // Human needs position 2 to win

        RuleBasedAIStrategy ai = new RuleBasedAIStrategy();
        int move = ai.GetAIMove(board);

        // AI should block at position 2
        Assert.AreEqual(2, move);
    }

    [Test]
    public void Test_AI_TakesWinningMove()
    {
        // AI (player 2) has two in a row at positions 0 and 1
        int[] board = new int[9];
        board[0] = 2; board[1] = 2; // AI needs position 2 to win

        RuleBasedAIStrategy ai = new RuleBasedAIStrategy();
        int move = ai.GetAIMove(board);

        // AI should take winning move at position 2
        Assert.AreEqual(2, move);
    }

    [Test]
    public void Test_AI_BlocksDifferentPattern()
    {
        // Human (player 1) has two in a column
        int[] board = new int[9];
        board[0] = 1; board[3] = 1; // Human needs position 6 to win

        RuleBasedAIStrategy ai = new RuleBasedAIStrategy();
        int move = ai.GetAIMove(board);

        // AI should block at position 6
        Assert.AreEqual(6, move);
    }

    [Test]
    public void Test_WinChecker_ReturnsCorrectPattern()
    {
        int[] board = new int[9];
        board[0] = 1; board[1] = 1; board[2] = 1;

        WinChecker checker = new WinChecker();
        int pattern = checker.GetWinningPattern(board);

        Assert.AreEqual(0, pattern); // First pattern (top row)
    }
}