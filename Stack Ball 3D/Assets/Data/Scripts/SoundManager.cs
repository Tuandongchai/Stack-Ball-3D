using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get =>instance; }
    private AudioSource audioSource;
    protected bool sound = true;
    public bool Sound { get => sound; }

    private void Awake()
    {
        MakeSingleton();
        audioSource=GetComponent<AudioSource>();
    }
    protected virtual void MakeSingleton()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public virtual void SoundOnOff()
    {
        sound =!sound;
    }
    public virtual void PlaySoundFX(AudioClip clip, float voulme)
    {
        if(sound) audioSource.PlayOneShot(clip, voulme);
    }
}
