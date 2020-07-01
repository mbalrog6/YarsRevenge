using System;
using UnityEngine;

public class PlayerInput : IPlayerInput
{
    public float Horizontal => Input.GetAxis("Horizontal");
    public float Vertical => Input.GetAxis("Vertical");

    public Direction PlayerDirection { get; private set; } = Direction.NONE;
    public Direction LastPlayerDirection { get; private set; } = Direction.NONE;
    public Direction LastPlayerFacing { get; private set; } = Direction.NONE;

    public void Tick()
    {
        LastPlayerDirection = PlayerDirection;
        
        if (LastPlayerDirection != Direction.NONE)
        {
            LastPlayerFacing = LastPlayerDirection;
        }
        
        PlayerDirection = GetDirection();
    }

    private Direction GetDirection()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        int directionFlag = 0;

        if (horizontal > 0)
        {
            directionFlag |= 1;
        }else if (horizontal < 0)
        {
            directionFlag |= 2;
        }
        if (vertical > 0)
        {
            directionFlag |= 4;
        }else if (vertical < 0)
        {
            directionFlag |= 8;
        }

        switch (directionFlag)
        {
            case 4:
                return Direction.NORTH;
            case 5:
                return Direction.NORTH_EAST;
            case 1:
                return Direction.EAST;
            case 9:
                return Direction.SOUTH_EAST;
            case 8:
                return Direction.SOUTH;
            case 10:
                return Direction.SOUTH_WEST;
            case 2:
                return Direction.WEST;
            case 6:
                return Direction.NORTH_WEST;
            default:
                return Direction.NONE;
        }
    }
    
}
