using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button[] cells;
    public TMP_Text[] cellTexts;
    public TMP_Text titleText;
    public GameObject[] strikeLines;

    private int currentPlayer = 0; // 0 = X, 1 = O
    private bool gameOver = false;
    private int[] board = new int[9];

    private int[] winCheck = new int[3];

    int[,] winPatterns = new int[,]
    {
        {0,1,2}, {3,4,5}, {6,7,8},
        {0,3,6}, {1,4,7}, {2,5,8},
        {0,4,8}, {2,4,6}
    };

    void Start()
    {
        ResetGame();
    }

    // CLICK CELL
    public void OnCellClicked(int index)
    {
        if (gameOver || board[index] != 0)
            return;

        // Block AI clicking
        // Block human ONLY when AI turn
        if (GameModeManager.instance.IsAI() && currentPlayer == 1)
        {
            Debug.Log("Waiting for AI...");
            return;
        }

        MakeMove(index);

        //  AI TURN
        if (GameModeManager.instance.IsAI() && currentPlayer == 1 && !gameOver)
        {
            Invoke(nameof(AIMove), 0.5f);
        }
    }

    // MAKE MOVE
    void MakeMove(int index)
    {
        board[index] = currentPlayer == 0 ? 1 : 2;

        cellTexts[index].text = currentPlayer == 0 ? "X" : "O";
        cellTexts[index].color = currentPlayer == 0 ? Color.red : Color.blue;

        cells[index].interactable = false;

        CheckWinner();

        if (!gameOver)
        {
            currentPlayer = 1 - currentPlayer;
            UpdateTitle();
        }
    }

    // TITLE
    void UpdateTitle()
    {
        if (GameModeManager.instance.IsAI())
        {
            if (currentPlayer == 0)
                titleText.text = "Your Turn";
            else
                titleText.text = GameModeManager.instance.IsHard()
                    ? "AI Thinking (Hard)"
                    : "AI Thinking (Easy)";
        }
        else
        {
            titleText.text = currentPlayer == 0
                ? "Player X Turn"
                : "Player O Turn";
        }
    }

    // CHECK WIN
    void CheckWinner()
    {
        for (int i = 0; i < 8; i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (board[a] != 0 && board[a] == board[b] && board[b] == board[c])
            {
                gameOver = true;
                strikeLines[i].SetActive(true);

                int winner = board[a]; // 1 = X, 2 = O

                // AI MODE WIN TEXT
                if (GameModeManager.instance.IsAI())
                {
                    if (winner == 1)
                    {
                        titleText.text = " You Win!";
                    }
                    else
                    {
                        titleText.text = " AI Wins!";
                    }
                }
                //  PVP MODE WIN TEXT
                else
                {
                    titleText.text = winner == 1
                        ? " Player X Wins!"
                        : " Player O Wins!";
                }

                Debug.Log("🏆 Winner: " + (winner == 1 ? "X" : "O"));
                return;
            }
        }

        //  DRAW CHECK
        bool full = true;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                full = false;
                break;
            }
        }

        if (full && !gameOver)
        {
            gameOver = true;
            titleText.text = " Draw!";
            Debug.Log("Game ended in draw");
        }
    }

    //  AI MOVE
    void AIMove()
    {
        if (gameOver) return;

        int move;

        if (GameModeManager.instance.IsHard())
            move = GetBestMove();   // Hard AI
        else
            move = GetRandomMove(); // Easy AI

        MakeMove(move);
    }

    //  EASY AI
    int GetRandomMove()
    {
        int count = 0;

        for (int i = 0; i < board.Length; i++)
            if (board[i] == 0) count++;

        int randomIndex = Random.Range(0, count);

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                if (randomIndex == 0)
                    return i;

                randomIndex--;
            }
        }

        return 0;
    }

    //  HARD AI (WIN → BLOCK → RANDOM)
    int GetBestMove()
    {
        int move;

        move = FindMove(2); // WIN
        if (move != -1) return move;

        move = FindMove(1); // BLOCK
        if (move != -1) return move;

        return GetRandomMove();
    }

    // FIND MOVE
    int FindMove(int player)
    {
        for (int i = 0; i < 8; i++)
        {
            winCheck[0] = winPatterns[i, 0];
            winCheck[1] = winPatterns[i, 1];
            winCheck[2] = winPatterns[i, 2];

            int a = winCheck[0], b = winCheck[1], c = winCheck[2];

            if (board[a] == player && board[b] == player && board[c] == 0) return c;
            if (board[a] == player && board[c] == player && board[b] == 0) return b;
            if (board[b] == player && board[c] == player && board[a] == 0) return a;
        }

        return -1;
    }

    // RESET
    public void ResetGame()
    {
        gameOver = false;
        currentPlayer = 0;

        for (int i = 0; i < 9; i++)
        {
            board[i] = 0;
            cellTexts[i].text = "";
            cellTexts[i].color = Color.white;
            cells[i].interactable = true;
        }

        foreach (var line in strikeLines)
            line.SetActive(false);

        UpdateTitle();
    }

    //  MENU
    public void BackToMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    // Replace only the ApplySettings method and add a new method for UI refresh
    public void ApplySettings()
    {
        Debug.Log(" Settings changed mid-game");

        // Reset any pending AI moves
        CancelInvoke();

        // Update UI
        UpdateTitle();

        // Refresh UI elements to show correct mode
        RefreshUIForMode();

        // If switched to AI and it's AI's turn → trigger move
        if (GameModeManager.instance.IsAI() && currentPlayer == 1 && !gameOver)
        {
            Invoke(nameof(AIMove), 0.3f);
        }
    }

    // New method to refresh UI based on current mode
    void RefreshUIForMode()
    {
        // Make sure buttons are interactable based on game state
        for (int i = 0; i < cells.Length; i++)
        {
            if (!gameOver && board[i] == 0)
            {
                // If it's AI mode and it's AI's turn, disable all buttons
                if (GameModeManager.instance.IsAI() && currentPlayer == 1)
                {
                    cells[i].interactable = false;
                }
                else
                {
                    cells[i].interactable = true;
                }
            }
        }
        }
    }