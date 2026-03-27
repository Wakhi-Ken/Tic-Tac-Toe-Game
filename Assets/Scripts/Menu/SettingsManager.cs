using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Game Mode Settings")]
    public TMP_Dropdown gameModeDropdown;

    [Header("Difficulty Settings")]
    public Toggle easyToggle;
    public Toggle hardToggle;

    [Header("Audio Settings")]
    public Slider volumeSlider;

    private AudioManager audioManager;

    void Start()
    {
        // Find AudioManager in scene (might be in DontDestroyOnLoad)
        FindAudioManager();

        // Load current settings
        LoadSettingsToUI();

        // Add listeners with safety checks
        if (gameModeDropdown != null)
            gameModeDropdown.onValueChanged.AddListener(OnGameModeChanged);

        if (easyToggle != null)
            easyToggle.onValueChanged.AddListener(OnEasyToggle);

        if (hardToggle != null)
            hardToggle.onValueChanged.AddListener(OnHardToggle);

        if (volumeSlider != null)
        {
            // Remove existing listeners to avoid duplicates
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    void FindAudioManager()
    {
        // Try to find AudioManager in the scene
        audioManager = Object.FindFirstObjectByType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogWarning("⚠️ AudioManager not found in scene! Creating temporary one...");
            // Create a GameObject with AudioManager if it doesn't exist
            GameObject audioManagerGO = new GameObject("AudioManager");
            audioManager = audioManagerGO.AddComponent<AudioManager>();
        }
    }

    void LoadSettingsToUI()
    {
        // Load game mode
        if (gameModeDropdown != null)
        {
            int savedMode = PlayerPrefs.GetInt("GameMode", 0);
            gameModeDropdown.value = savedMode;
        }

        // Load difficulty
        if (easyToggle != null && hardToggle != null)
        {
            int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 0);
            easyToggle.isOn = (savedDifficulty == 0);
            hardToggle.isOn = (savedDifficulty == 1);
        }

        // Load volume
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            volumeSlider.value = savedVolume;

            // Apply volume immediately
            if (audioManager != null)
            {
                audioManager.SetVolume(savedVolume);
            }
            else
            {
                // Fallback: set volume directly
                AudioListener.volume = savedVolume;
            }
        }
    }

    void OnGameModeChanged(int value)
    {
        if (GameModeManager.instance != null)
            GameModeManager.instance.SetGameMode(value);
        else
            Debug.LogWarning("⚠️ GameModeManager.instance is null!");
    }

    void OnEasyToggle(bool isOn)
    {
        if (isOn && GameModeManager.instance != null)
            GameModeManager.instance.SetEasy(true);
    }

    void OnHardToggle(bool isOn)
    {
        if (isOn && GameModeManager.instance != null)
            GameModeManager.instance.SetHard(true);
    }

    void OnVolumeChanged(float value)
    {
        Debug.Log("🔊 Volume slider changed to: " + value);

        // Try to use AudioManager
        if (audioManager == null)
        {
            FindAudioManager();
        }

        if (audioManager != null)
        {
            audioManager.SetVolume(value);
        }
        else
        {
            // Fallback: set volume directly if AudioManager doesn't exist
            Debug.LogWarning("⚠️ AudioManager not found, setting volume directly");
            AudioListener.volume = value;
            PlayerPrefs.SetFloat("musicVolume", value);
            PlayerPrefs.Save();
        }
    }

    // Call this when returning to menu to refresh settings
    void OnEnable()
    {
        Debug.Log("🔄 Settings Menu Enabled - Refreshing UI");
        LoadSettingsToUI();

        // Refresh GameModeManager settings
        if (GameModeManager.instance != null)
            GameModeManager.instance.RefreshSettings();

        // Re-find AudioManager
        FindAudioManager();
    }

    // Clean up listeners when disabled
    void OnDisable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        if (gameModeDropdown != null)
            gameModeDropdown.onValueChanged.RemoveListener(OnGameModeChanged);
        if (easyToggle != null)
            easyToggle.onValueChanged.RemoveListener(OnEasyToggle);
        if (hardToggle != null)
            hardToggle.onValueChanged.RemoveListener(OnHardToggle);
    }
}