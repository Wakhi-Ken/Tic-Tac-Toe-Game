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

    [Header("Platform-Specific UI")]
    public GameObject quitButton; // Only appears on non-WebGL

    private AudioManager audioManager;

    void Start()
    {
        FindAudioManager();
        LoadSettingsToUI();
        AddListeners();
        SetupPlatformSpecificUI();
    }

    void SetupPlatformSpecificUI()
    {
#if UNITY_WEBGL
            // Hide quit button on WebGL
            if (quitButton != null)
                quitButton.SetActive(false);
                
            // Add instruction text
            GameObject instructions = GameObject.Find("InstructionText");
            if (instructions != null)
            {
                TMP_Text text = instructions.GetComponent<TMP_Text>();
                if (text != null) text.text = "Web Version - Click to play";
            }
#elif UNITY_ANDROID || UNITY_IOS
        // Make UI elements larger for touch
        if (gameModeDropdown != null)
        {
            RectTransform rect = gameModeDropdown.GetComponent<RectTransform>();
            if (rect != null) rect.sizeDelta = new Vector2(200, 50);
        }

        if (volumeSlider != null)
        {
            RectTransform rect = volumeSlider.GetComponent<RectTransform>();
            if (rect != null) rect.sizeDelta = new Vector2(300, 30);
        }

        // Add mobile instructions
        GameObject instructions = GameObject.Find("InstructionText");
        if (instructions != null)
        {
            TMP_Text text = instructions.GetComponent<TMP_Text>();
            if (text != null) text.text = "Mobile Version - Tap to play";
        }
#elif UNITY_STANDALONE
            // Desktop version
            GameObject instructions = GameObject.Find("InstructionText");
            if (instructions != null)
            {
                TMP_Text text = instructions.GetComponent<TMP_Text>();
                if (text != null) text.text = "Desktop Version - Click to play";
            }
#endif
    }

    void FindAudioManager()
    {
        audioManager = Object.FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found! Creating one...");
            GameObject audioManagerGO = new GameObject("AudioManager");
            audioManager = audioManagerGO.AddComponent<AudioManager>();
        }
    }

    void LoadSettingsToUI()
    {
        if (gameModeDropdown != null)
        {
            int savedMode = PlayerPrefs.GetInt("GameMode", 0);
            gameModeDropdown.value = savedMode;
        }

        if (easyToggle != null && hardToggle != null)
        {
            int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 0);
            easyToggle.isOn = (savedDifficulty == 0);
            hardToggle.isOn = (savedDifficulty == 1);
        }

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            volumeSlider.value = savedVolume;
            if (audioManager != null) audioManager.SetVolume(savedVolume);
            else AudioListener.volume = savedVolume;
        }
    }

    void AddListeners()
    {
        if (gameModeDropdown != null)
            gameModeDropdown.onValueChanged.AddListener(OnGameModeChanged);
        if (easyToggle != null)
            easyToggle.onValueChanged.AddListener(OnEasyToggle);
        if (hardToggle != null)
            hardToggle.onValueChanged.AddListener(OnHardToggle);
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    void OnGameModeChanged(int value)
    {
        if (GameModeManager.instance != null)
            GameModeManager.instance.SetGameMode(value);
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
        if (audioManager == null) FindAudioManager();
        if (audioManager != null) audioManager.SetVolume(value);
        else
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat("musicVolume", value);
            PlayerPrefs.Save();
        }
    }

    void OnEnable()
    {
        LoadSettingsToUI();
        if (GameModeManager.instance != null)
            GameModeManager.instance.RefreshSettings();
        FindAudioManager();
    }

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