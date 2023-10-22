using UnityEngine;
using System.Collections;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip backgroundMusic; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void StartBackgroundMusic()
    {
        if (backgroundMusic != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
}
