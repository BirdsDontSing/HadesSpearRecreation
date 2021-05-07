using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        //create audio source
        GameObject audioObject = new GameObject("2D Audio " + clip.ToString());
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        //configure it
        audioSource.clip = clip;
        audioSource.volume = volume;

        //play the audio
        audioSource.Play();

        //destroy it when done
        Object.Destroy(audioObject, clip.length);

        //return it
        return audioSource;
    }
}
