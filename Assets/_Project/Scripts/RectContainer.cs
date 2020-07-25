using UnityEngine;

public class RectContainer
{ 
    private GameObject _targetObject;
    private Rect _bounds;
    private float _halfWidth;
    private float _halfHeight;

    public Rect Bounds => _bounds;

    public RectContainer( GameObject entity, float x, float y, float width, float height )
    {
        _targetObject = entity;
        _bounds = new Rect(x, y, width, height );
        _halfHeight = height / 2f;
        _halfWidth = width / 2f;
    }

    public void UpdateToTargetPosition()
    {
        if (_targetObject == null) return;
        
        UpdatePosition( _targetObject.transform.position);
    }

    public void UpdatePosition(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x - _halfWidth, position.y - _halfHeight, 0f);
        _bounds.position = newPosition;
    }

    public void SetRectContainer(float x, float y, float width, float height)
    {
        _bounds.x = x;
        _bounds.y = y;
        _bounds.width = width;
        _bounds.height = height; 
        _halfHeight = height / 2f;
        _halfWidth = width / 2f;
    }

}
