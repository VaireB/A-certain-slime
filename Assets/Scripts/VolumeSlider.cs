using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        // Initialize volume slider with current game volume
        slider.value = AudioListener.volume;
    }

    // Method to adjust game volume based on slider value
    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // Method to set the volume to maximum when the slider is fully to the right
    public void SetMaxVolume(float volume)
    {
        if (slider.value == slider.maxValue)
        {
            AudioListener.volume = 1.0f;
        }
    }
}
