using UnityEngine;
using UnityEngine.UI;

public class DirectionalRotator : IRotator
{
    private Player player;
    private Transform playerTransform;
    
    public DirectionalRotator(Player player)
    {
        this.player = player;
        playerTransform = player.transform;
    }
    
    public void Tick()
    {
        var angle = Vector3.Angle(player.LastFacingDirectionVector, Vector3.up);
        switch (player.PlayerInput.Inputs.LastFacingDirection)
        {
            case CardinalDirection.WEST:
            case CardinalDirection.SOUTH_EAST:
            case CardinalDirection.NORTH_EAST:
                angle = -angle;
                break;
        }
        playerTransform.rotation = Quaternion.Euler(0f, 0f, angle );
    }
}
