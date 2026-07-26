using UnityEngine;
using UnityEngine.Audio;

public class SimpleAudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // Exposed parameter names in the mixer
    private const string MASTER_VOL = "MasterVolume";
    private const string MUSIC_VOL = "MusicVolume";
    private const string SFX_VOL = "SFXVolume";

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MASTER_VOL, LinearToDecibel(value));
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MUSIC_VOL, LinearToDecibel(value));
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SFX_VOL, LinearToDecibel(value));
    }

    private float LinearToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }
}