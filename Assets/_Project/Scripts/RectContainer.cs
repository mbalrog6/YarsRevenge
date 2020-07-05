using UnityEngine;

public class RectContainer
{
    private Rect _bounds;
    public Rect Bounds => _bounds;
    
    public RectContainer( float x, float y, float width, float height )
    {
        _bounds = new Rect(x, y, width, height );
    }

    public void UpdatePosition(Vector3 position)
    {
        _bounds.position = position;
    }

}
