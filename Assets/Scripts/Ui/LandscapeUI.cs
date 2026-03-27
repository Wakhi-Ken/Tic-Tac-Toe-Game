using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandscapeUI : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public RectTransform gameBoard;
    public TextMeshProUGUI titleText;

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        SetupLandscapeUI();
#endif
    }

    void SetupLandscapeUI()
    {
        // Set canvas to match landscape
        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1f; // Match height for landscape
        }

        // Position game board for landscape
        if (gameBoard != null)
        {
            gameBoard.anchorMin = new Vector2(0.15f, 0.1f);
            gameBoard.anchorMax = new Vector2(0.85f, 0.9f);
            gameBoard.offsetMin = Vector2.zero;
            gameBoard.offsetMax = Vector2.zero;
        }

        // Adjust title text
        if (titleText != null)
        {
            titleText.fontSize = 40;
            RectTransform titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.95f);
        }

        Debug.Log("Landscape UI setup complete");
    }
}