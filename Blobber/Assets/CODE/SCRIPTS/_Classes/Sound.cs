using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
    
    public string name = "Sound";

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.8f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public AudioMixerGroup mixerGroup;

    [HideInInspector] public AudioSource source;

}
