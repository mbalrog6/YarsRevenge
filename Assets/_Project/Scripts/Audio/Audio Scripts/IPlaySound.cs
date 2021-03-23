using UnityEngine;

public interface IPlaySound
{
    void Play(AudioSource source);
    void PlayOneShot(AudioSource source);
}
