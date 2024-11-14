using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundManager : MonoBehaviour
{
    public static ClickSoundManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private bool isSoundEnabled = true;

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
        }
    }

    public void PlayClickSound()
    {
        if (isSoundEnabled && audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void SetSoundEnabled(bool enabled)
    {
        isSoundEnabled = enabled;
    }
}
