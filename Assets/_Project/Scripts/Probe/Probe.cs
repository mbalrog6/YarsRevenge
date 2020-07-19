using System;
using UnityEngine;

public class Probe : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Warlord warlord;
    [SerializeField] private float respawnTimer;

    public event Action OnDie;
    public event Action OnRespawn;
    
    public RectContainer ProbeRectContainer => _probeRectContainer;
    public bool IsDead => _isDead; 
    public float Radius => radius;

    private bool _isDead;
    private RectContainer _probeRectContainer;
    private float _timer = 0f;
    private GameObject _visual; 


    private void Awake()
    {
        _isDead = false;
        _visual = transform.GetChild(0).gameObject;
        _probeRectContainer = new RectContainer(this.gameObject, .125f, .125f, .5f, .5f);
    }

    private void Update()
    {
        if (IsDead)
        {
            if (Time.time > _timer && Warlord.State == WarlordState.Idle)
            {
                Respawn();
            }
            else
            {
                return;
            }
        }
        _probeRectContainer.UpdateToTargetPosition();
    }

    public void Die()
    {
        _isDead = true;
        _visual.SetActive(false);
        _timer = Time.time + respawnTimer;
        OnDie?.Invoke();
    }

    public void Respawn()
    {
        _isDead = false;
        _visual.SetActive(true);
        transform.position = warlord.transform.position;
        OnRespawn?.Invoke();
    }

    private void OnDrawGizmos()
    {

        if (_probeRectContainer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMax, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMax, 0f));
        
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
