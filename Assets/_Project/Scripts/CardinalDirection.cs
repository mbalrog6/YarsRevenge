using System;
using UnityEngine;

[Flags] public enum CardinalDirection
{
  NORTH = 4, 
  NORTH_EAST = 5,
  EAST = 1,
  SOUTH_EAST = 9,
  SOUTH = 8,
  SOUTH_WEST = 10,
  WEST = 2,
  NORTH_WEST = 6,
  NONE = 0, 
}

public static class CardinalDirections
{
  #region Unit Vector Definitions ...
  private static Vector3 _north = new Vector3(0f, 1f, 0f);
  private static Vector3 _east = new Vector3(1f, 0f, 0f);
  private static Vector3 _south = new Vector3(0f, -1f, 0f);
  private static Vector3 _west = new Vector3(-1f, 0f, 0f);
  private static Vector3 _northEast = new Vector3(1f, 1f, 0f).normalized;
  private static Vector3 _southEast = new Vector3(1f, -1f, 0f).normalized;
  private static Vector3 _southWest = new Vector3(-1f, -1f, 0f).normalized;
  private static Vector3 _northWest =new Vector3(-1f, 1f, 0f).normalized;
  public static Vector3 NorthVector => _north;
  public static Vector3 EastVector => _east;
  public static Vector3 SouthVector => _south;
  public static Vector3 WestVector => _west;
  public static Vector3 NorthEastVector => _northEast;
  public static Vector3 SouthEastVector => _southEast;
  public static Vector3 SouthWestVector => _southWest;
  public static Vector3 NorthWestVector => _northWest;
  #endregion
  
  public static Vector3 GetUnitVectorFromCardinalDirection(CardinalDirection cardinalDirection)
  {
    switch (cardinalDirection)
    {
      case CardinalDirection.NORTH:
        return NorthVector;
      case CardinalDirection.SOUTH:
        return SouthVector;
      case CardinalDirection.EAST:
        return EastVector;
      case CardinalDirection.WEST:
        return WestVector;
      case CardinalDirection.NORTH_WEST:
        return NorthWestVector;
      case CardinalDirection.SOUTH_WEST:
        return SouthWestVector;
      case CardinalDirection.NORTH_EAST:
        return NorthEastVector;
      case CardinalDirection.SOUTH_EAST:
        return SouthEastVector;
      default:
        return Vector3.zero;
    }
  }
  
  public static CardinalDirection GetDirectionFromInput( float verticalAxis, float horizontalAxis)
  {
        
    CardinalDirection directionFlag = CardinalDirection.NONE;

    if (horizontalAxis > 0)
    {
      directionFlag |= CardinalDirection.EAST;
    }
    else if (horizontalAxis < 0)
    {
      directionFlag |= CardinalDirection.WEST;
    }
    
    if (verticalAxis > 0)
    {
      directionFlag |= CardinalDirection.NORTH;
    }
    else if (verticalAxis < 0)
    {
      directionFlag |= CardinalDirection.SOUTH;
    }

    return directionFlag;
  }

  public static CardinalDirection GetDirectionFromVector(Vector3 directionVector)
  {
    directionVector.Normalize();
    var horizontal = directionVector.x;
    var vertical = directionVector.y;

    return GetDirectionFromInput(vertical, horizontal);
  }

  public static CardinalDirection GetDirectionFromVector(Vector2 directionVector)
  {
    directionVector.Normalize();
    var horizontal = directionVector.x;
    var vertical = directionVector.y;
    
    return GetDirectionFromInput(vertical, horizontal);
  }
}