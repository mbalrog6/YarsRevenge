using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public int CurrentButtonIndex => _buttonIndex;

    public bool HasFocus
    {
        get { return _hasFocus; }
        set { _hasFocus = value;  }
    }

    [SerializeField] private bool _hasFocus = true;

    private List<IMenuButton> _buttons;
    private int _buttonIndex;
    
    private MenuInputDTO _menuInputDTO;

    private void Awake()
    {
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
    }

    private void Update()
    {
        if (!HasFocus) return; 
        
        _menuInputDTO = InputManager.Instance.GetMenuInputDTO();

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