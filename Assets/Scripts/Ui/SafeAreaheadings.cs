using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    public RectTransform mainPanel;

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        ApplySafeArea();
#endif
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        mainPanel.anchorMin = anchorMin;
        mainPanel.anchorMax = anchorMax;

        Debug.Log($"Safe Area applied: {safeArea}");
    }
}