using UnityEngine;

public class ResetAllSettings : MonoBehaviour
{
    public void ResetToDefault()
    {
        // 🧨 1. Clear saved PlayerPrefs (NEW KEYS)
        PlayerPrefs.DeleteKey("GameMode");
        PlayerPrefs.DeleteKey("Difficulty");
        PlayerPrefs.Save();

        // 🎮 2. Reset runtime values
        if (GameModeManager.instance != null)
        {
            // Default values
            GameModeManager.instance.currentMode = GameModeManager.GameMode.PvP;
            GameModeManager.instance.difficulty = GameModeManager.Difficulty.Easy;

            Debug.Log("🔄 Settings reset to default (PvP + Easy)");

            // 🔥 Apply instantly to game
            GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.ApplySettings();
        }
    }
}