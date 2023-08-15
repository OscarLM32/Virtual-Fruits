using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundsSO sounds;

    private void Awake()
    {
        GameObject soundObject = new GameObject("Sounds");
        soundObject.transform.position = transform.position;
        soundObject.transform.SetParent(transform);
        
        foreach (var s in sounds.sounds)
        {
            s.source = soundObject.AddComponent<AudioSource>();
            while (s.source == null)
            {
                s.source = soundObject.GetComponent<AudioSource>();
            }
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    public void Play(string name)
    {
        Sound s = sounds.GetSound(name);
        s.source.Play();
    }
}
