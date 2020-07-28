using UnityEngine;

public class MenuInputInterpreter
{
    private int _lastVertical;
    private int _vertical;
    private MenuInputDTO menuInput;

    public MenuInputDTO Transform(InputDTO playerInputDTO)
    {
        _lastVertical = _vertical;
        _vertical = playerInputDTO.Vertical > 0.1 ? 1 : 0;
        _vertical = playerInputDTO.Vertical < -0.1 ? -1 : _vertical;

        if (_lastVertical == _vertical)
        {
            menuInput.Vertical = 0;
        }
        else
        {
            if (_vertical != 0)
            {
                menuInput.Vertical = _vertical > 0 ? 1 : -1;
            }
            else
            {
                menuInput.Vertical = 0;
            }
        }

        menuInput.MousePosition = Input.mousePosition;

        menuInput.Button = playerInputDTO.FireButton;
        menuInput.Paused = playerInputDTO.Paused;

        return menuInput; 
    }
}

public struct MenuInputDTO
{
    public int Vertical;
    public bool Button;
    public Vector2 MousePosition;
    public bool Paused;
}