using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Settings")]
    [SerializeField] private bool persistBetweenScenes = true;

    [Header("Sounds")]
    public Sound[] sounds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (persistBetweenScenes)
        {
            DontDestroyOnLoad(gameObject);
        }

        InitializeSounds();
    }

    private void InitializeSounds()
    {
        if (sounds == null)
        {
            sounds = Array.Empty<Sound>();
            return;
        }

        foreach (Sound sound in sounds)
        {
            if (sound == null)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(sound.name))
            {
                Debug.LogWarning(
                    $"An unnamed sound exists on {gameObject.name}."
                );
            }

            if (sound.source == null)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
            }

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            // Play manually after all settings have been applied.
            sound.source.playOnAwake = false;

            if (sound.playOnAwake && sound.clip != null)
            {
                sound.source.Play();
            }
        }
    }

    public void Play(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            return;
        }

        if (sound.clip == null)
        {
            Debug.LogWarning(
                $"Sound \"{soundName}\" has no AudioClip assigned."
            );

            return;
        }

        sound.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            return;
        }

        sound.source.Stop();
    }

    public bool IsPlaying(string soundName)
    {
        Sound sound = FindSound(soundName);

        return sound != null &&
               sound.source != null &&
               sound.source.isPlaying;
    }

    private Sound FindSound(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName))
        {
            return null;
        }

        Sound sound = Array.Find(
            sounds,
            entry =>
                entry != null &&
                string.Equals(
                    entry.name,
                    soundName,
                    StringComparison.OrdinalIgnoreCase
                )
        );

        if (sound == null)
        {
            Debug.LogWarning(
                $"Sound \"{soundName}\" was not found."
            );
        }

        return sound;
    }
}