using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("BackgroundMusic");
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, s => s.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PitchUp(string soundName, float pitch)
    {
        Sound s = Array.Find(sounds, s => s.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found on pitch up!");
            return;
        }

        if (pitch < 0.5f)
        {
            s.pitch = 0.5f;
            s.source.pitch = s.pitch;
        }
        else if (s.pitch < 3)
        {
            s.pitch = pitch;
            s.source.pitch = s.pitch;

        }
    }

    public void Reset(string soundName)
    {
        Sound s = Array.Find(sounds, s => s.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found on reset!");
            return;
        }

        s.pitch = 1;
        s.source.pitch = s.pitch;
    }
    
}
