
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Play the background audio
        PlayBackgroundAudio();
    }

    private void PlayBackgroundAudio()
    {
        if (audioSource != null)
        {
            // Make sure the audio source is set to loop so that the background music keeps playing
            audioSource.loop = true;

            // Play the background music
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource component not found. Make sure it's attached to the GameObject.");
        }
    }
}
