
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

namespace YarsRevenge._Project.Scripts.Menu
{
    public class PlayMenuButton : ShakeButton
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _imageHighlight;
        [SerializeField] private ActiveText _activeText;
        
        private Color _color;
        private bool _initalized = false;

        public override void OnEnter()
        {
            base.OnEnter();
            _color = _image.color;
            _image.color = Color.white;
            _imageHighlight.enabled = true;
            _activeText.IsActive = true;
            _activeText.UpdateUIText();

            if (_initalized == false)
            {
                _initalized = true;
                return;
            }

            MasterAudio.PlaySoundAndForget("Rebound");
        }

        public override void OnExit()
        {
            _image.color = _color;
            _imageHighlight.enabled = false;
            _activeText.IsActive = false;
            _activeText.UpdateUIText();
        }

        public override void OnClick()
        {
            MasterAudio.PlaySoundAndForget("Click Ui");
            GameStateMachine.Instance.ChangeTo = States.LOADING;
        }
    }
}
