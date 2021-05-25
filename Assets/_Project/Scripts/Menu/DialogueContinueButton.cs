using UnityEngine;
using YarsRevenge._Project.Audio;

public class DialogueContinueButton : MonoBehaviour, IMenuButton
{
    public int Index => _index;
    protected RectTransform _UIRectTransform;
    private int _index;
    private float _timer;
    
    [Header("Audio")] [SerializeField] private SimpleAudioEvent buttonClick;
    private AudioSource _audioSource;

    private void Awake()
    {
        _UIRectTransform = GetComponent<RectTransform>();
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnClick()
    {
        /*
        if (gameObject.transform.gameObject.activeSelf == false)
            return;
        if (Time.time > _timer)
        {
            Mediator.Instance.Publish(new NextDialogueChunk());
            _timer = Time.time + .3f; 
            buttonClick.PlayOneShot(_audioSource);
        }

        _UIRectTransform.DORewind();
        _UIRectTransform.DOPunchScale(new Vector3(0f, -0.1f, 0f), .2f, 50, 1f);
        */
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public int MouseOverMenuButton(Vector2 mousePosition)
    {
        var point = _UIRectTransform.InverseTransformPoint(mousePosition);
        if (_UIRectTransform.rect.Contains(point))
        {
            return Index; 
        }

        return -1; 
    }
}
