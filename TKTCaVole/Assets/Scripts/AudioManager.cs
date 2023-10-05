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
        foreach (var clip in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();

            source.clip = clip.Clip;
            
            sources.Add(clip.Key,source);
            
        }
    }

    public void PlaySound(string key)
    {
        if(!sources.ContainsKey(key)) return;

        var source = sources[key];

        source.Play();
    }
}
