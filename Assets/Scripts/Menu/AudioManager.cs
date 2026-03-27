using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    private static AudioManager instance;

    void Awake()
    {
        //  Prevent destruction + duplicates
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Find slider if not assigned in inspector
        if (volumeSlider == null)
        {
            volumeSlider = GetComponent<Slider>();
            if (volumeSlider == null)
                volumeSlider = GetComponentInChildren<Slider>();
        }

        Load();

        // Apply saved volume
        if (volumeSlider != null)
        {
            AudioListener.volume = volumeSlider.value;

            // Add listener for slider changes (remove existing to avoid duplicates)
            volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    public void ChangeVolume()
    {
        if (volumeSlider != null)
        {
            AudioListener.volume = volumeSlider.value;
            Save();
        }
    }

    private void Load()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }

    private void Save()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
            PlayerPrefs.Save();
        }
    }

    // Public method to get current volume
    public float GetVolume()
    {
        return AudioListener.volume;
    }

    // Public method to set volume from anywhere
    public void SetVolume(float volume)
    {
        Debug.Log(" Setting volume to: " + volume);

        // Clamp volume between 0 and 1
        volume = Mathf.Clamp01(volume);

        // Update audio listener volume
        AudioListener.volume = volume;

        // Update slider if it exists
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();

        Debug.Log(" Volume saved: " + volume);
    }

    private void OnSliderValueChanged(float value)
    {
        ChangeVolume();
    }
}