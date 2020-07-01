using UnityEngine;

public static class VectorExtentions
{
    public static Vector3 North(this Vector3 vector)
    {
        return new Vector3(0f, 1f, 0f);
    }

    public static Vector3 South(this Vector3 vector)
    {
        return new Vector3(0f, -1f, 0f);
    }
    
    public static Vector3 East(this Vector3 vector)
    {
        return new Vector3(1f, 0f, 0f);
    }
    
    public static Vector3 West(this Vector3 vector)
    {
        return new Vector3(-1f, 0f, 0f);
    }
    
    public static Vector3 NorthEast (this Vector3 vector)
    {
        var returnVector = new Vector3(1f, 1f, 0f).normalized;
        return returnVector;
    }
    
    public static Vector3 NorthWest(this Vector3 vector)
    {
        var returnVector = new Vector3(-1f, 1f, 0f).normalized;
        return returnVector;
    }
    
    public static Vector3 SouthEast (this Vector3 vector)
    {
        var returnVector = new Vector3(1f, -1f, 0f).normalized;
        return returnVector;
    }
    
    public static Vector3 SouthWest (this Vector3 vector)
    {
        var returnVector = new Vector3(-1f, -1f, 0f).normalized;
        return returnVector;
    }
}
