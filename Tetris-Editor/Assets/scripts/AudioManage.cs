using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public AudioClip[] audios;
    public float volume = 1;
    [HideInInspector]
    public AudioSource[] audioSources;

    private void Start()
    {
        audioSources = new AudioSource[audios.Length];
        for (int i = 0; i < audios.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audios[i];
            audioSources[i].volume = volume;
        }
    }
    public void ChangeVolume(float to)
    {
        for (int i = 0; i < audios.Length; i++)
        {
            audioSources[i].volume = to;
        }
    }
}
