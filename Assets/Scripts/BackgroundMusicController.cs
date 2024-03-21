using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if the AudioSource is not null and the audio clip is assigned
        if (audioSource != null && audioSource.clip != null)
        {
            // Start playing the background music
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music audio source or clip is missing.");
        }
    }
}
