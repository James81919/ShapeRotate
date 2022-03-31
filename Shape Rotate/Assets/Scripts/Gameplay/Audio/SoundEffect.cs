using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;

    public AudioClip clip;

    [Range(0, 1)] public float volume = 1;
    [Min(0.1f)] public float minPitch;
    [Min(0.1f)] public float maxPitch;

    public bool loop = false;

    [HideInInspector] public AudioSource source;

    public float GetRandomPitch()
    {
        return Random.Range(minPitch, maxPitch);
    }
}
