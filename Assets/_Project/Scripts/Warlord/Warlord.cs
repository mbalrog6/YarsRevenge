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
    [SerializeField] private int ammoProvided;
    [SerializeField] private float ammoCooldown;
    public static WarlordState State { get; set; } = WarlordState.Idle;
    public bool CanGetAmmo => Time.time > _ammoProvidedTimer;
    public RectContainer WarlordRectContainer => _warlordBounds;

    private RectContainer _warlordBounds;
    private float _ammoProvidedTimer;

    private void Awake()
    {
        _warlordBounds = new RectContainer( this.gameObject, 1f, 1f, 1f, 1f);
    }

    private void Update()
    {
        _warlordBounds.UpdateToTargetPosition();
    }

    public int GetAmmo()
    {
        _ammoProvidedTimer = Time.time + ammoCooldown;
        return ammoProvided;
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
