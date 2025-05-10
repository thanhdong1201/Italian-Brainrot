using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundfxSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        SetVolume(savedVolume);
    }
    public void SetVolume(float value)
    {
        musicSource.volume = value;
        soundfxSource.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    public void PlaySound(AudioClip clip)
    {
        soundfxSource.PlayOneShot(clip);
    }
    public void PlayCorrectSound()
    {
        PlaySound(correctSound);
    }
    public void PlayIncorrectSound()
    {
        PlaySound(incorrectSound);
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop());
        }
    }
    public void StopMusicNow()
    {
        musicSource.Stop();
    }
    private IEnumerator FadeOutAndStop()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / 0.5f; // Fade out over 2 seconds  
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume for future use  
    }
    public void PauseMusic(bool pause)
    {
        if (pause)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }
}
