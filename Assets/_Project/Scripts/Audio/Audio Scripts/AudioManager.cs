using System.Collections.Generic;
using UnityEngine;

namespace YarsRevenge._Project.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private int initialNumberOfAudioSources = 1;

        public static AudioManager Instance => _instance;
        private static AudioManager _instance;
    
        private Dictionary<int, AudioSource> _audioSources;
        private AudioSource _omniPresentAudioSource;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }

            _omniPresentAudioSource = gameObject.AddComponent<AudioSource>();
            _audioSources = new Dictionary<int, AudioSource>();

            for (int i = 0; i < initialNumberOfAudioSources; i++)
            {
                _audioSources.Add(i, gameObject.AddComponent<AudioSource>());
            }
        
        }

        public AudioSource RequestAudioSource(int id)
        {
            if (_audioSources.ContainsKey(id))
            {
                return _audioSources[id];
            }

            var audioSource = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(id, audioSource);
            return audioSource;
        }

        public AudioSource RequestOneShotAudioSource()
        {
            return _audioSources[0];
        }

        public void PauseAudio()
        {
            foreach (var audioSource in _audioSources.Values)
            {
                audioSource.Pause();
            }
        }

        public void UnPauseAudio()
        {
            foreach (var audioSource in _audioSources.Values)
            {
                audioSource.UnPause();
            }
        }

        public AudioSource RequestOmniPresentAudioSource()
        {
            return _omniPresentAudioSource;
        }

        public void StopAllSound()
        {
            foreach (var audioSource in _audioSources.Values)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }

            _omniPresentAudioSource.Stop();
        }
    }
}
