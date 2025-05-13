using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set;}
    [Header("FlappyBird Sound Effects")]
    [SerializeField] private AudioSource soundFx;
    [SerializeField] private AudioClip[] flapClips;
    [SerializeField] private AudioSource flapSource;
    [SerializeField] private AudioClip collisionClip;
    [SerializeField] private AudioClip scoreClip;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip themeMusic;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        } 
    }
    public void PlayFlapSound()
    {
        if (flapClips.Length == 0 || flapSource == null) return;
        AudioClip clip = flapClips[Random.Range(0, flapClips.Length)];
        flapSource.Stop();
        flapSource.clip = clip;
        flapSource.Play();
    }
    public void PlayCollisionSound()
    {
        soundFx.PlayOneShot(collisionClip);
    }
    public void PlayScoreSound()
    {
        soundFx.PlayOneShot(scoreClip);
    }

    public void PlayThemeMusic()
    {
        if (musicSource == null || themeMusic == null) return;
        musicSource.clip = themeMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopThemeMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
