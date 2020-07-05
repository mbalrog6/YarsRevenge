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
}
