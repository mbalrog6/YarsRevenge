using UnityEngine;

public abstract class PlaySound : ScriptableObject, IPlaySound
{
    public abstract void Play(AudioSource source);
    public abstract void PlayOneShot(AudioSource source);
}
