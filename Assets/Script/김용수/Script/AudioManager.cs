using UnityEngine;
using System.Collections.Generic;

// AudioManager 클래스 (이전과 동일)
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;
    public AudioSource effectsSource;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            audioClips.Add(s.name, s.clip);
        }
    }

    public void PlaySound(string soundName, bool loop = false, float pitch = 1f)
    {
        if (audioClips.ContainsKey(soundName))
        {
            if (loop)
            {
                effectsSource.clip = audioClips[soundName];
                effectsSource.loop = true;
                effectsSource.pitch = pitch;
                effectsSource.Play();
            }
            else
            {
                effectsSource.pitch = pitch;
                effectsSource.PlayOneShot(audioClips[soundName]);
            }
        }
        else
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
        }
    }

    public void StopSound(string soundName)
    {
        if (effectsSource.clip != null && effectsSource.clip.name == soundName)
        {
            effectsSource.Stop();
            effectsSource.loop = false;
        }
    }

    public bool IsPlaying(string soundName)
    {
        return effectsSource.isPlaying && effectsSource.clip != null && effectsSource.clip.name == soundName;
    }

    public void SetPitch(string soundName, float pitch)
    {
        if (effectsSource.clip != null && effectsSource.clip.name == soundName)
        {
            effectsSource.pitch = pitch;
        }
    }
}
