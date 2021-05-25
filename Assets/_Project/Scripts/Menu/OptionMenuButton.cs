using System.Collections;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class OptionMenuButton : ShakeButton
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageHighlight;
    [SerializeField] private ActiveText _activeText;
    [SerializeField] private TextMeshProUGUI _text;
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
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        _text.text = "Sorry!";
        yield return new WaitForSeconds(1f);
        _activeText.UpdateUIText();
    }
}
