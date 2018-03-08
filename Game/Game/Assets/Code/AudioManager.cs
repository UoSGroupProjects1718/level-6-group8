using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip sound;
    public EventType type;
}

public class AudioManager : MonoBehaviour
{
    /* Singleton */
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    //! Audio source component
    private AudioSource source;

    //! List of our sounds
    [SerializeField]
    private List<Sound> sounds;

    /// <summary>
    /// Initializes the singleton
    /// </summary>
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            source = GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Plays a given SoundType
    /// </summary>
    /// <param name="type">The sound to play</param>
    public void PlaySound(EventType type)
    {
        var ac = GetSound(type);
        if (ac != null)
            source.PlayOneShot(ac, 1);
    }

    /// <summary>
    /// Returns the Audio clip of the sound for a given Sound type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private AudioClip GetSound(EventType type)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == type)
            {
                return sound.sound;
            }
        }
        return null;
    }
}
