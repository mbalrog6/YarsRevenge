using UnityEngine;

public readonly struct Dimensions
{
    public readonly int x;
    public readonly int y;
    public readonly int z;

    public Dimensions(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }

    public static implicit operator Vector3(Dimensions dim) => dim.GetVector3();
    public static explicit operator Dimensions(Vector3 dim) => new Dimensions((int)dim.x, (int)dim.y, (int)dim.z);

    public static implicit operator Vector2(Dimensions dim) => dim.GetVector2();
    public static explicit operator Dimensions(Vector2 dim) => new Dimensions((int)dim.x, (int)dim.y, 0);
}