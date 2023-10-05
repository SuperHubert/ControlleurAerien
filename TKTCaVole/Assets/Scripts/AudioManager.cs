using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        [field:SerializeField] public string Key { get; private set; }
        [field:SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float Volume { get; private set; } = 1;
    }
    
    
    [SerializeField] private List<Sound> clips = new();
    private Dictionary<string, AudioSource> sources = new();

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateAudioSources();
    }

    private void CreateAudioSources()
    {
        sources.Clear();
        foreach (var sound in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = sound.Volume;
            source.clip = sound.Clip;
            
            sources.Add(sound.Key,source);
            
        }
    }

    public void PlaySound(string key)
    {
        if(!sources.ContainsKey(key)) return;

        var source = sources[key];

        source.Play();
    }
}
