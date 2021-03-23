using DG.Tweening;
using UnityEngine;

public abstract class ShakeButton : MonoBehaviour, IMenuButton
{
    public int Index => _index;
    protected RectTransform _UIRectTransform;
    private int _index;

    [SerializeField] protected float _shakeDuration = .2f;
    [SerializeField] protected float _shakeStrength = 5f;
    [SerializeField] protected int _vibroto = 50; 

    public virtual void Awake()
    {
        _UIRectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnEnter()
    {
        _UIRectTransform.DOShakePosition(_shakeDuration, _shakeStrength,_vibroto);
        _UIRectTransform.DOShakeRotation(_shakeDuration, _shakeStrength, _vibroto);
    }

    public abstract void OnExit();

    public abstract void OnClick();

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
