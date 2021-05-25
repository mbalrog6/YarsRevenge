using UnityEngine;

public class IonZone : MonoBehaviour
{
    [SerializeField] private float _widthOffset = 0f;
    private Transform _ionZoneVisual;
    private float _width;
    private float _height;

    public float Width
    {
        get => _width;
        set { _width = value; SetRectContainer(); }
    }

    public float Height
    {
        get => _height;
        set { _height = value; SetRectContainer(); }
    }

    public RectContainer IonRectContainer => _rectContainer;

    private RectContainer _rectContainer;

    private void Awake()
    {
        _ionZoneVisual = GetComponentInChildren<Transform>();
    }

    private void Start()
    {
        _height = ScreenHelper.Instance.ScreenBounds.height;
        _width = 6f;
        _rectContainer = new RectContainer(this.gameObject, 0, 0, _width + _widthOffset, _height);
        Vector3 position = new Vector3(
            ScreenHelper.Instance.ScreenBounds.xMin + (ScreenHelper.Instance.ScreenBounds.width / 2.8f),
            ScreenHelper.Instance.ScreenBounds.center.y,
            0f);
        transform.position = position;
        
        _ionZoneVisual.localScale = new Vector3(_width, _height, 1f);
    }

    private void OnEnable()
    {
        _height = ScreenHelper.Instance.ScreenBounds.height;
        _width = 3f;
        _rectContainer = new RectContainer(this.gameObject, 0, 0, _width + _widthOffset, _height);
        Vector3 position = new Vector3(
            ScreenHelper.Instance.ScreenBounds.xMin + (ScreenHelper.Instance.ScreenBounds.width / 2.8f),
            ScreenHelper.Instance.ScreenBounds.center.y,
            0f);
        transform.position = position;

        _ionZoneVisual.localScale = new Vector3(_width, _height, 1f);
    }

    private void Update()
    {
        _rectContainer.UpdateToTargetPosition();
    }

    private void SetRectContainer()
    {
        _rectContainer.SetRectContainer(0, 0, _width + _widthOffset, _height);
        //_ionZoneVisual.localScale = new Vector3(_width * 0.4166f, _height * 0.2272f, 1f);
        _ionZoneVisual.localScale = new Vector3(_width, _height, 1f);
    }
    private void OnDrawGizmos()
    {
        if (_rectContainer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMax, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMax, 0f));
    }
}
