using Game.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ATTACK,
    DASH,
    FOOT,
    ABILITY
}
public class PoolItemSound : PoolItemBase
{
    private AudioSource _audioSource;
    [SerializeField] private SoundType _soundType;
    [SerializeField] private AssetsSoundSO _soundAssets;

    public override void Spawn()
    {
        PlaySound();
    }
    public override void Recycle()
    {
        DisAbleSelf();
    }

    private void PlaySound()
    {
        _audioSource.clip = _soundAssets.GetAudioClip(_soundType);
        _audioSource.Play();
        StartRecycle();
    }
    private void StartRecycle()
    {
        TimerManager.MainInstance.TryGetOneTimer(0.3f, Recycle);
    }
    private void DisAbleSelf()
    {
        _audioSource.Stop();
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
