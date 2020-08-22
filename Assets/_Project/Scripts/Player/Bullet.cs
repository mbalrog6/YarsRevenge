using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Player player;
    [SerializeField] private float radius = .2f;

    private Barrier2 barrier;
    
    [Header("Audio")]
    [SerializeField] private SimpleAudioEvent _splatterSound;
    private AudioSource _audioSource;
    private Vector3 _fireDirection;

    public bool HasBeenFired { get; private set; }

    public event Action OnDisabled;

    private void Awake()
    {
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
    }

    private void Start()
    {
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
    }

    public void DisableBullet()
    {
        HasBeenFired = false;
        gameObject.SetActive(false);
        OnDisabled?.Invoke();
    }

    public void FireBullet()
    {
        gameObject.SetActive(true);
        HasBeenFired = true;
        _fireDirection = CardinalDirections.GetUnitVectorFromCardinalDirection(player.PlayerInputDTO.LastFacingDirection);
        transform.position = spawnPoint.position;
    }

    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE)
        {
            return;
        }

        if (HasBeenFired)
        {
            Move();
            CheckForCollisionWithBarrior();
            CheckForOffScreen();
        }
    }

    private void CheckForOffScreen()
    {
        if (!ScreenHelper.Instance.ScreenBounds.Contains(transform.position))
        {
            DisableBullet();
        }
    }

    private void CheckForCollisionWithBarrior()
    {
        var collision = barrier.BarrierRectContainer.Bounds.Contains(transform.position);
        if (collision)
        {
            var index = barrier.GetCellFromVector3(transform.position);
            if (index.HasValue)
            {
                _splatterSound.PlayOneShot(_audioSource);
                barrier.DisableCellsInPlusPattern(index.Value);
                DisableBullet();
                GameStateMachine.Instance.ChangeTo = States.BRIEF_PAUSE;
            }
        }
    }

    private void Move()
    {
        if (_fireDirection == Vector3.zero)
        {
            DisableBullet();
            return;
        }
        transform.position += _fireDirection * (speed * Time.deltaTime);
    }

    private void UpdateBarrier(Barrier2 barrier)
    {
        this.barrier = barrier;
    }

    public void Reset()
    {
        DisableBullet();
    }
}