using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GameLogic;
using AI;

public class GameManager : MonoBehaviour
{
    public Button[] cells;
    public TMP_Text[] cellTexts;
    public TMP_Text titleText;
    public GameObject[] strikeLines;

    private int currentPlayer = 0; // 0 = X, 1 = O
    private bool gameOver = false;

    // New: Pure logic classes
    private BoardState boardState;
    private WinChecker winChecker;
    private IAIStrategy aiStrategy;

    void Awake()
    {
        // Initialize logic classes
        boardState = new BoardState();
        winChecker = new WinChecker();
        aiStrategy = new RuleBasedAIStrategy();

        // Subscribe to events
        GameEvents.OnGameReset += HandleReset;
    }

    void OnDestroy()
    {
        GameEvents.OnGameReset -= HandleReset;
    }

    void Start()
    {
        // Lock to landscape on mobile
#if UNITY_ANDROID || UNITY_IOS
        LockToLandscape();
#endif

        ResetGame();

        // Mobile optimization for landscape
#if UNITY_ANDROID || UNITY_IOS
        SetupMobileUI();
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    void LockToLandscape()
    {
        // Force landscape orientation
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Disable portrait auto-rotation
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        // Enable landscape auto-rotation
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        // Keep screen awake during gameplay
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Debug.Log("Game locked to landscape mode on mobile");
    }

    void SetupMobileUI()
    {
        // Make buttons larger for landscape touch input
        foreach (var cell in cells)
        {
            RectTransform rect = cell.GetComponent<RectTransform>();
            if (rect != null)
            {
                // Landscape mode: wider buttons for easier tapping
                rect.sizeDelta = new Vector2(110, 100);
            }
        }

        // Make text slightly larger for landscape readability
        foreach (var text in cellTexts)
        {
            text.fontSize = 42;
        }

        // Adjust title text for landscape
        if (titleText != null)
        {
            titleText.fontSize = 40;
            RectTransform titleRect = titleText.GetComponent<RectTransform>();
            if (titleRect != null)
            {
                titleRect.anchorMin = new Vector2(0.5f, 0.9f);
                titleRect.anchorMax = new Vector2(0.5f, 0.95f);
            }
        }

        Debug.Log("Mobile UI optimized for landscape");
    }
#endif

    public void OnCellClicked(int index)
    {
        if (gameOver || !boardState.IsCellEmpty(index)) return;

        if (GameModeManager.instance.IsAI() && currentPlayer == 1)
        {
            Debug.Log("Waiting for AI...");
            return;
        }

        MakeMove(index);

        if (GameModeManager.instance.IsAI() && currentPlayer == 1 && !gameOver)
        {
            Invoke(nameof(AIMove), 0.5f);
        }
    }

    void MakeMove(int index)
    {
        int playerValue = currentPlayer == 0 ? 1 : 2;

        // Update logic
        boardState.SetCell(index, playerValue);

        // Update UI
        cellTexts[index].text = currentPlayer == 0 ? "X" : "O";
        cellTexts[index].color = currentPlayer == 0 ? Color.red : Color.blue;
        cells[index].interactable = false;

        // Check game state
        CheckGameState();

        if (!gameOver)
        {
            currentPlayer = 1 - currentPlayer;
            UpdateTitle();
            GameEvents.MoveMade(index);
        }
    }

    void CheckGameState()
    {
        int[] board = boardState.GetBoard();
        int winner = winChecker.CheckWinner(board);

        if (winner != 0)
        {
            gameOver = true;
            int patternIndex = winChecker.GetWinningPattern(board);
            if (patternIndex >= 0 && patternIndex < strikeLines.Length)
                strikeLines[patternIndex].SetActive(true);

            if (GameModeManager.instance.IsAI())
            {
                titleText.text = winner == 1 ? "You Win!" : "AI Wins!";
            }
            else
            {
                titleText.text = winner == 1 ? "Player X Wins!" : "Player O Wins!";
            }

            GameEvents.GameWon(winner);
            return;
        }

        if (winChecker.IsBoardFull(board))
        {
            gameOver = true;
            titleText.text = "Draw!";
            GameEvents.GameDraw();
        }
    }

    void AIMove()
    {
        if (gameOver) return;

        int[] board = boardState.GetBoard();
        int move = aiStrategy.GetAIMove(board);

        if (move != -1 && boardState.IsCellEmpty(move))
        {
            MakeMove(move);
        }
    }

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
            titleText.text = currentPlayer == 0 ? "Player X Turn" : "Player O Turn";
        }
    }

    public void ResetGame()
    {
        gameOver = false;
        currentPlayer = 0;

        boardState.Reset();

        for (int i = 0; i < 9; i++)
        {
            cellTexts[i].text = "";
            cellTexts[i].color = Color.white;
            cells[i].interactable = true;
        }

        foreach (var line in strikeLines)
            line.SetActive(false);

        UpdateTitle();
        GameEvents.GameReset();
    }

    void HandleReset()
    {
        CancelInvoke();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void ApplySettings()
    {
        Debug.Log("Settings changed mid-game");
        CancelInvoke();
        UpdateTitle();
        RefreshUIForMode();

        if (GameModeManager.instance.IsAI() && currentPlayer == 1 && !gameOver)
        {
            Invoke(nameof(AIMove), 0.3f);
        }
    }

    void RefreshUIForMode()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!gameOver && boardState.IsCellEmpty(i))
            {
                cells[i].interactable = !(GameModeManager.instance.IsAI() && currentPlayer == 1);
            }
        }
    }
}