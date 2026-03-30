using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;

    //  GAME MODE
    public enum GameMode
    {
        PvP,
        PvAI
    }

    [Header("Game Mode")]
    public GameMode currentMode = GameMode.PvP;

    //  DIFFICULTY (VISIBLE IN INSPECTOR)
    public enum Difficulty
    {
        Easy,
        Hard
    }

    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Easy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //  DROPDOWN (0 = PvP, 1 = PvAI)
    public void SetGameMode(int index)
    {
        switch (index)
        {
            case 0:
                currentMode = GameMode.PvP;
                break;
            case 1:
                currentMode = GameMode.PvAI;
                break;
        }

        PlayerPrefs.SetInt("GameMode", index);
        PlayerPrefs.Save();

        Debug.Log("Game Mode: " + currentMode);

        ApplyToGame();
        NotifySettingsChanged();
    }

    // EASY CHECKBOX
    public void SetEasy(bool isOn)
    {
        if (!isOn) return;

        difficulty = Difficulty.Easy;
        PlayerPrefs.SetInt("Difficulty", 0);
        PlayerPrefs.Save();

        Debug.Log("Difficulty: EASY");

        ApplyToGame();
        NotifySettingsChanged();
    }

    //  HARD CHECKBOX
    public void SetHard(bool isOn)
    {
        if (!isOn) return;

        difficulty = Difficulty.Hard;
        PlayerPrefs.SetInt("Difficulty", 1);
        PlayerPrefs.Save();

        Debug.Log("Difficulty: HARD");

        ApplyToGame();
        NotifySettingsChanged();
    }

    //  CHECK IF AI MODE
    public bool IsAI()
    {
        return currentMode == GameMode.PvAI;
    }

    // CHECK IF HARD MODE
    public bool IsHard()
    {
        return difficulty == Difficulty.Hard;
    }

    // APPLY SETTINGS TO GAME
    void ApplyToGame()
    {
        // Find GameManager in the current scene
        GameManager gm = Object.FindFirstObjectByType<GameManager>();

        if (gm != null)
        {
            gm.ApplySettings();
        }
    }

    // Notify any UI elements that settings changed
    void NotifySettingsChanged()
    {
        // For now, just update the game if it exists
        ApplyToGame();
    }

    // 💾 LOAD SAVED SETTINGS
    void LoadSettings()
    {
        currentMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        difficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty", 0);
    }

    // Public method to refresh settings from PlayerPrefs
    public void RefreshSettings()
    {
        LoadSettings();
        NotifySettingsChanged();
    }
}