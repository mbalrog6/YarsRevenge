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
    [SerializeField] private int _scoreForSwirl;
    [SerializeField] private int _scoreForWarlord;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SimpleAudioEvent swirlAttack;
    [SerializeField] private SimpleAudioEvent chargeUpSound;
    public static WarlordState State { get; set; } = WarlordState.Idle;
    public bool CanGetAmmo => Time.time > _ammoProvidedTimer;
    public RectContainer WarlordRectContainer => _warlordBounds;
    public int Score => Warlord.State == WarlordState.LaunchedTowardsPlayer ? _scoreForSwirl : _scoreForWarlord;

    private RectContainer _warlordBounds;
    private float _ammoProvidedTimer;

    private void Awake()
    {
        _warlordBounds = new RectContainer( this.gameObject, 1f, 1f, 1f, 1f);
        audioSource = GetComponent<AudioSource>();
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

    public void Die()
    {
        State = WarlordState.Dead;
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PlayLaunchAtSound()
    {
        if (audioSource.isPlaying)
            return;
        
        swirlAttack.Play(audioSource);
    }

    public void PlayChargingSound()
    {
        if (audioSource.isPlaying)
            return;
        chargeUpSound.Play(audioSource);
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
