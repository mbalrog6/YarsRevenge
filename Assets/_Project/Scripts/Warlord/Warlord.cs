using UnityEngine;

public enum WarlordState
{
    Idle, 
    ChargeUp, 
    LaunchedTowardsPlayer,
    Dead,
}
public class Warlord : MonoBehaviour
{
    public static WarlordState State { get; set; } = WarlordState.Idle;
    public Rect Bounds => _warlordBounds.Bounds;

    private RectContainer _warlordBounds;

    private void Awake()
    {
        _warlordBounds = new RectContainer( this.gameObject, 1f, 1f, 1f, 1f);
    }

    private void Update()
    {
        _warlordBounds.UpdateToTargetPosition();
    }
    
    private void OnDrawGizmos()
    {
        if (_warlordBounds == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMax, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMax, 0f));
    }
}
