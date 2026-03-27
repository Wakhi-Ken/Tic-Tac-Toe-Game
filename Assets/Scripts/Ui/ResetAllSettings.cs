using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetAllSettings : MonoBehaviour
{
    [Header("UI References - Optional")]
    public TMP_Dropdown gameModeDropdown;
    public Toggle easyToggle;
    public Toggle hardToggle;
    public Slider volumeSlider;

    public void ResetToDefault()
    {
        // Clear saved PlayerPrefs
        PlayerPrefs.DeleteKey("GameMode");
        PlayerPrefs.DeleteKey("Difficulty");
        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.Save();

        // Reset runtime values in GameModeManager
        if (GameModeManager.instance != null)
        {
            GameModeManager.instance.currentMode = GameModeManager.GameMode.PvP;
            GameModeManager.instance.difficulty = GameModeManager.Difficulty.Easy;

            Debug.Log("Settings reset to default (PvP + Easy)");
        }

        // Reset Audio
        AudioListener.volume = 1f;

        // Update UI elements immediately (if assigned)
        UpdateUIDropdowns();

        // Apply to game if playing
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.ApplySettings();
            Debug.Log(" Settings applied to current game");
        }

        Debug.Log(" All settings reset to default successfully!");
    }

    void UpdateUIDropdowns()
    {
        // Update Game Mode Dropdown
        if (gameModeDropdown != null)
        {
            gameModeDropdown.value = 0; // PvP
            gameModeDropdown.RefreshShownValue();
            Debug.Log(" Game mode dropdown updated to PvP");
        }

        // Update Difficulty Toggles
        if (easyToggle != null && hardToggle != null)
        {
            easyToggle.isOn = true;
            hardToggle.isOn = false;
            Debug.Log("Difficulty toggles updated to Easy");
        }

        // Update Volume Slider
        if (volumeSlider != null)
        {
            volumeSlider.value = 1f;
            Debug.Log("Volume slider updated to 100%");
        }

        // Force UI refresh
        if (gameModeDropdown != null)
            gameModeDropdown.onValueChanged.Invoke(0);

        if (easyToggle != null)
            easyToggle.onValueChanged.Invoke(true);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.Invoke(1f);
    }
}