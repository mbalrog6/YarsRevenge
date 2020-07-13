using UnityEngine;
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
        if (player.PlayerInputDTO.Direction == CardinalDirection.NONE)
            return; 
        
        var angle = Vector3.Angle(player.DirectionVector, Vector3.up);
        switch (player.PlayerInputDTO.Direction)
        {
            case CardinalDirection.EAST:
            case CardinalDirection.SOUTH_EAST:
            case CardinalDirection.NORTH_EAST:
                angle = -angle;
                break;
        }
        playerTransform.rotation = Quaternion.Euler(0f, 0f, angle );
    }
}
