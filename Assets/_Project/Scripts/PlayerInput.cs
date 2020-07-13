using UnityEngine;

public struct InputDTO
{
    public float Vertical;
    public float Horizontal;
    public CardinalDirection Direction; 
    public CardinalDirection LastDirection;
    public CardinalDirection LastFacingDirection;

    public InputDTO( float horizontal, float vertical, CardinalDirection direction, CardinalDirection lastDirection, CardinalDirection lastFacingDirection )
    {
        this.Vertical = vertical;
        this.Horizontal = horizontal;
        this.Direction = direction;
        this.LastDirection = lastDirection;
        this.LastFacingDirection = lastFacingDirection;
    }
    
    public override string ToString()
    {
        var inputsSting = $"Vertical = {Vertical}\r\nHorizontal = {Horizontal}\r\nDirection = {Direction.ToString()}\r\nLastDirection = {LastDirection.ToString()}\r\nLastFacingDirection = {LastFacingDirection.ToString()} ";
        return inputsSting;
    }
}
public class PlayerInput : IPlayerInput
{
    private InputDTO _inputDTO;
    public InputDTO Inputs => _inputDTO;

    public PlayerInput()
    {
        _inputDTO = new InputDTO(0f, 0f,
            CardinalDirection.NONE,
            CardinalDirection.NONE,
            CardinalDirection.NONE);
    }
    public void Tick()
    {
       SetInput(ref _inputDTO);
    }

    public void SetInput(ref InputDTO input)
    {
        input.Vertical = Input.GetAxisRaw("Vertical");
        input.Horizontal = Input.GetAxisRaw("Horizontal");

        input.LastDirection = Inputs.Direction;
        if (input.LastDirection != CardinalDirection.NONE)
        {
            input.LastFacingDirection = input.LastDirection;
        }
        input.Direction = CardinalDirections.GetDirectionFromInput(input.Vertical, input.Horizontal);

        _inputDTO = input;
    }
    
    public void CopyDTO(ref InputDTO inputDTO)
    {
        inputDTO.Vertical = _inputDTO.Vertical;
        inputDTO.Horizontal = _inputDTO.Horizontal;
        inputDTO.Direction = _inputDTO.Direction;
        inputDTO.LastDirection = _inputDTO.LastDirection;
        inputDTO.LastFacingDirection = _inputDTO.LastFacingDirection;
    }

    public void SetInput(InputDTO inputs)
    {
        _inputDTO.Direction = inputs.Direction;
        _inputDTO.LastDirection = inputs.LastDirection;
        _inputDTO.LastFacingDirection = inputs.LastFacingDirection;
        _inputDTO.Vertical = inputs.Vertical;
        _inputDTO.Horizontal = inputs.Horizontal;
    }
}
