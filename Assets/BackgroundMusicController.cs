using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utils.Extensions;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip[] bgSoundClips;

    public AudioSource bgAudioSource;


    private void Start()
    {
        AudioClip rand = bgSoundClips.RandomItem();
        bgAudioSource.clip = rand;
        bgAudioSource.Play();
    }

    private void Update()
    {

        if(!bgAudioSource.isPlaying)
            bgAudioSource.DOFade(0, 1f).OnComplete(PlayMusic);
        
    }

    private void PlayMusic()
    {
        AudioClip rand = bgSoundClips.RandomItem();
        bgAudioSource.clip = rand;
        bgAudioSource.Play();
        bgAudioSource.DOFade(1, 1f);

    }
    
}
