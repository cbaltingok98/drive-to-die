using _project.Scripts.Enums;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundType name;
    public AudioClip Clip;
    public bool themeSong;
    [Range(0f, 1f)] public float Volume;
    [Range(.1f, 3f)] public float Pitch;

    public bool loop;
    [HideInInspector] public AudioSource source;
} 