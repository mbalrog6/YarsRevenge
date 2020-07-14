using UnityEngine;

public class ScreenHelper : MonoBehaviour
{
    private static float _height;
    private static float _bottom;
    private static float _width;
    private static float _left;

    private static Camera _camera;
    private static Rect _screenBounds;
    private static ScreenHelper _instance;
    public static ScreenHelper Instance => _instance;
    public Rect ScreenBounds => _screenBounds;

    private void Awake()
    {
        _camera = Camera.main;
        FindBoundries();
        _screenBounds = new Rect(_left, _bottom, _width, _height);

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        
        _instance = this;
    }

    void FindBoundries()
    {
        _width = 1 / (_camera.WorldToViewportPoint(new Vector3(1,1,0)).x - .5f);
        _height = 1 / (_camera.WorldToViewportPoint(new Vector3(1,1,0)).y - .5f);

        _bottom = -(_height / 2);
        _left = -(_width / 2);
    }

    private void Update()
    {
        FindBoundries();
        _screenBounds.x = _left;
        _screenBounds.y = _bottom;
        _screenBounds.width = _width;
        _screenBounds.height = _height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3( _screenBounds.xMin, _screenBounds.yMin, 0f), new Vector3( _screenBounds.xMin, _screenBounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _screenBounds.xMax, _screenBounds.yMin, 0f), new Vector3( _screenBounds.xMax, _screenBounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _screenBounds.xMin, _screenBounds.yMin, 0f), new Vector3( _screenBounds.xMax, _screenBounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3( _screenBounds.xMin, _screenBounds.yMax, 0f), new Vector3( _screenBounds.xMax, _screenBounds.yMax, 0f));
    }
}
