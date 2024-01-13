using DG.Tweening;
using Game.Tool.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    
}

public enum BGM
{
    FOREST,
    DARK_FOREST
}

public class AudioManager : Singleton<AudioManager>
{
    public static AudioManager instance;

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;

    [SerializeField] private float fadeDuration = 0.75f;
    private float originalMusicVol;

    private void Start()
    {
        originalMusicVol = musicPlayer.volume;
    }

    public void PlayMusic(BGM id, bool loop=true, bool fade=false, float volume=1f)
    {
        if (musicPlayer.clip == musicClips[(int)id])
        {
            return;
        }
        StartCoroutine(PlayerMusicAsync(id, loop, fade, volume));
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void PlaySE(SFX id, bool pauseMusic=false, float volume=1f)
    {
        AudioClip clip = sfxClips[(int)id];
        if (pauseMusic)
        {
            musicPlayer.Pause();
            StartCoroutine(UnPauseMusic(clip.length * 0.75f));
        }
        sfxPlayer.pitch = 1f;
        sfxPlayer.PlayOneShot(clip, volume);
    }

    private IEnumerator UnPauseMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        musicPlayer.volume = 0;
        musicPlayer.UnPause();
        musicPlayer.DOFade(originalMusicVol, fadeDuration);  
    }

    private IEnumerator PlayerMusicAsync(BGM id, bool loop, bool fade, float volume=1f)
    {
        if (fade)
        {
            yield return musicPlayer.DOFade(0, fadeDuration).WaitForCompletion();
        }

        musicPlayer.clip = musicClips[(int)id]; ;
        musicPlayer.loop = loop;
        musicPlayer.volume = volume;
        musicPlayer.Play();

        if (fade)
        {
            yield return musicPlayer.DOFade(originalMusicVol, fadeDuration).WaitForCompletion();
        }
    }

}
