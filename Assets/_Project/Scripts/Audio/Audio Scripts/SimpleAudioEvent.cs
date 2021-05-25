using Audio;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : PlaySound
{
    [SerializeField] private AudioClip[] clips = new AudioClip[0];

    [SerializeField] private RangedFloat volume = new RangedFloat(1, 1);
    [SerializeField] [MinMaxRange(0, 2f)] private RangedFloat pitch = new RangedFloat(1, 1);

    [SerializeField] [MinMaxRange(0, 1000f)]
    private RangedFloat distance = new RangedFloat(1, 1000f);

    [SerializeField] private AudioMixerGroup mixer;

    public override void Play(AudioSource source)
    {
        source.outputAudioMixerGroup = mixer;

        int clipIndex = UnityEngine.Random.Range(0, clips.Length);
        source.clip = clips[clipIndex];

        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.minDistance = distance.minValue;
        source.maxDistance = distance.maxValue;

        source.Play();
    }

    public override void PlayOneShot(AudioSource source)
    {
        source.outputAudioMixerGroup = mixer;

        int clipIndex = UnityEngine.Random.Range(0, clips.Length);
        //source.clip = clips[clipIndex];

        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.minDistance = distance.minValue;
        source.maxDistance = distance.maxValue;

        source.PlayOneShot(clips[clipIndex]);
    }
}