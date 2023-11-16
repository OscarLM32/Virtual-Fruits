using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class SoundsSO : ScriptableObject
{
    public Sound[] sounds;

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
}
