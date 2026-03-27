using UnityEngine;

public class OrientationLock : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        LockToLandscape();
#endif
    }

    void LockToLandscape()
    {
        // Lock orientation to landscape
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Disable auto-rotation
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Debug.Log("Game locked to landscape mode");
    }
}