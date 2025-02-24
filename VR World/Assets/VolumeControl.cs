using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour {
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start() {
        // Load saved volume (if any)
        if (PlayerPrefs.HasKey("MusicVolume")) {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        } else {
            volumeSlider.value = 1f; // Default volume
            SetVolume(1f);
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume) {
        // Convert linear slider value (0 - 1) to logarithmic (-80dB to 0dB)
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", dB);

        // Save volume setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}
