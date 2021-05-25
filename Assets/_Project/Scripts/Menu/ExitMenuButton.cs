using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenuButton : ShakeButton
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageHighlight;

    [SerializeField] private ActiveText _activeText;
    private Color _color;

    public override void OnEnter()
    {
        base.OnEnter();
        _color = _image.color;
        _image.color = Color.white;
        _imageHighlight.enabled = true;
        _activeText.IsActive = true;
        _activeText.UpdateUIText();

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
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
