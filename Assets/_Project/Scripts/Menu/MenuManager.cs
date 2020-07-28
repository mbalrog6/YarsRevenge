using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MenuManager : MonoBehaviour
{ 
    [SerializeField] private bool StartOnScreen = true;
    [SerializeField] private bool _hasFocus = true;
    public int CurrentButtonIndex => _buttonIndex;
    public MenuInputDTO MenuInputDTO => _menuInputDTO;

    public bool HasFocus
    {
        get { return _hasFocus; }
        set { _hasFocus = value;  }
    }

    private List<IMenuButton> _buttons;
    private int _buttonIndex;

    private MenuInputInterpreter _menuInputInterpreter;
    private MenuInputDTO _menuInputDTO;
    private RectTransform _rectTransform;
    private Vector3 _startPosition;
    private Vector3 _originPosition;
    private bool _initialized = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (StartOnScreen)
        {
            _startPosition = _rectTransform.localPosition;
        }
        else
        {
            _originPosition = _rectTransform.position;
            _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, Screen.height, 0);
            _startPosition = _rectTransform.position;
        }

        _menuInputInterpreter = new MenuInputInterpreter();
        _buttons = new List<IMenuButton>();
        _buttonIndex = 0;

        var menuButtons = GetComponentsInChildren<IMenuButton>();
        var index = 0;
        foreach (var button in menuButtons)
        {
            Add(button);
            button.SetIndex(index);
            index++;
        }
        
        _buttons[0].OnEnter();
        _initialized = true; 
    }

    private void Update()
    {
        if (!HasFocus) return;

        _menuInputDTO = _menuInputInterpreter.Transform(InputManager.Instance.PlayerInputDTO);

        if (_menuInputDTO.Vertical == 1)
        {
            Previous();
        }

        if (_menuInputDTO.Vertical == -1)
        {
            Next();
        }

        var index = GetMouseOverButtonIndex();
        if ( index != -1)
        {
            SetIndex(index);
        }

        if (_menuInputDTO.Button)
        {
            _buttons[_buttonIndex].OnClick();
        }
    }

    public void SetPosition(Vector3 position) => _rectTransform.localPosition = position;

    public void TweenToOrigin()
    {
        _rectTransform.DOMove(_originPosition, .5f).SetEase(Ease.InFlash);
    }

    public void TweenToPosition(Vector3 position, float duration)
    {
        _rectTransform.DOMove(position, duration);
    }

    public void TweenToStart()
    {
        _rectTransform.DOMove(_startPosition, .2f);
    }

    private int GetMouseOverButtonIndex()
    {
        int index;
        foreach (var button in _buttons)
        {
            index = button.MouseOverMenuButton(_menuInputDTO.MousePosition);
            if (index > -1 && index != CurrentButtonIndex)
            {
                return index;
            }
        }

        return -1; 
    }

    public void Next()
    {
        if (_buttons == null || _buttons.Count == 0)
            return; 
        
        _buttons[_buttonIndex].OnExit();
        _buttonIndex++;
        _buttonIndex = _buttonIndex % _buttons.Count;
        _buttons[_buttonIndex].OnEnter();
    }

    public void Previous()
    {
        if (_buttons == null || _buttons.Count == 0)
            return;

        _buttons[_buttonIndex].OnExit();
        
        _buttonIndex--;
        if (_buttonIndex < 0)
        {
            _buttonIndex = _buttons.Count - 1; 
        }
        
        _buttons[_buttonIndex].OnEnter();
    }

    public void Add(IMenuButton menuButton)
    {
        if (_buttons == null)
            return;
        
        _buttons.Add(menuButton);
    }

    public void Remove(IMenuButton menuButton)
    {
        if (_buttons == null)
            return;
        
        _buttons.Remove(menuButton);
    }

    public void SetIndex(int buttonIndex)
    {
        if (buttonIndex >= _buttons.Count || buttonIndex < 0)
            return;

        _buttons[_buttonIndex].OnExit(); 
        _buttonIndex = buttonIndex;
        _buttons[_buttonIndex].OnEnter();
    }

    public void ButtonClicked()
    {
        _buttons[_buttonIndex].OnClick();
    }
}