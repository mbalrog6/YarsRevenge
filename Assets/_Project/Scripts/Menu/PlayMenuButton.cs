using UnityEngine;
using UnityEngine.UI;
using YarsRevenge._Project.Audio;
using YarsRevenge._Project.Scripts.Audio.Audio_Scripts;

namespace YarsRevenge._Project.Scripts.Menu
{
    public class PlayMenuButton : ShakeButton
    {
        [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
        [SerializeField] private SimpleAudioEvent _onClickButtonSound;
        [SerializeField] private Image _image;
        [SerializeField] private Image _imageHighlight;
        private Color _color;
        private bool _initalized = false;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _color = _image.color;
            _image.color = Color.white;
            _imageHighlight.enabled = true;

            if (_initalized == false)
            {
                _initalized = true;
                return;
            }
            if (_audioSource != null && _onEnterButtonSound != null)
            {
                _onEnterButtonSound.PlayOneShot(_audioSource);
            }
        }

        public override void OnExit()
        {
            _image.color = _color;
            _imageHighlight.enabled = false;
        }

        public override void OnClick()
        {
            if (!_onClickButtonSound == null)
            {
                _onClickButtonSound.PlayOneShot(_audioSource);
            }
            GameStateMachine.Instance.ChangeTo = States.LOADING;
        }
    }
}
