using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Pools;
using UnityEngine;
using AudioType = UnityEngine.AudioType;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundTimerDictionary = new Dictionary<SoundType, float>();
        
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.loop;
            
            soundTimerDictionary.Add(s.name, 0f);
        }
    }
    
    public Sound[] sounds;

    private Dictionary<SoundType, float> soundTimerDictionary;

    private ObjectPooler objectPooler;

    private Coroutine themeCoroutine;

    private SoundType currentPlaying;
    
    public void Init()
    {
        objectPooler = ObjectPooler.Instance;
        PlayThemeSong();
    }


    public void PlayThemeSong()
    {
        if (!Constants.CanPlayMusic) return;

        var rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:
                currentPlaying = SoundType.Theme;
                Play(currentPlaying);
                break;
            case 2:
                currentPlaying = SoundType.Theme2;
                Play(currentPlaying);
                break;
            case 3:
                currentPlaying = SoundType.Theme3;
                Play(currentPlaying);
                break;
        }
    }


    public void StopThemeSong()
    {
        Stop(currentPlaying);
        if (themeCoroutine != null)
        {
            StopCoroutine(themeCoroutine);
        }
    }


    private IEnumerator RestartTheme(float wait)
    {
        yield return new WaitForSecondsRealtime(wait);
        PlayThemeSong();
    }

    public void Play(SoundType clipName)
    {
        if (!Constants.CanPlaySound) return;
        if (!CanPlaySound(clipName)) return;
        
        Sound s = Array.Find(sounds, sound => sound.name == clipName);

        if (s.themeSong)
        {
            
            if (themeCoroutine != null)
            {
                StopCoroutine(themeCoroutine);
            }

            s?.source.Play();
            themeCoroutine = StartCoroutine(RestartTheme(s.Clip.length));
            return;
        }
        
        objectPooler.SpawnAudioSource(ObjectType.Sound, s);
    }


    private bool CanPlaySound(SoundType sound)
    {
        switch (sound)
        {
            
            case SoundType.ZombieHit:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePLayer = soundTimerDictionary[sound];
                    float interval = 0.05f;
                    if (lastTimePLayer + interval < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            default: 
                return true;
        }
    }

    public void SetVolume(SoundType clipName, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        if (s == null) return;
        s.source.volume = volume;
    }

    public void Stop(SoundType clipName)
    {
        // if (PlayerPrefs.GetInt("game_sound") == 0) return;
        
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        s?.source.Stop();
    }

    public void StopAll()
    {
        // if (PlayerPrefs.GetInt("game_sound") == 0) return;
        
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }
    
}
