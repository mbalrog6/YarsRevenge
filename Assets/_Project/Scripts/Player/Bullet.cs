using System;
using DarkTonic.MasterAudio;
using UnityEngine;
using YarsRevenge._Project.Audio;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Player player;
    [SerializeField] private float radius = .2f;
    [SerializeField] private ParticleSystem bulletFX;
    [SerializeField] private ParticleSystem bulletExplosionFX;

    private Barrier2 barrier;
    private Vector3 _fireDirection;

    public bool HasBeenFired { get; private set; }

    public event Action OnDisabled;

    private void Awake()
    {
        if (bulletFX == null)
        {
            bulletFX = GetComponent<ParticleSystem>();
        }
        
        if (bulletExplosionFX == null)
        {
            bulletFX = GetComponent<ParticleSystem>();
        }
    }

    private void Start()
    {
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
    }

    public void DisableBullet()
    {
        HasBeenFired = false;
        gameObject.SetActive(false);
        bulletFX.Stop();
        OnDisabled?.Invoke();
    }

    public void FireBullet()
    {
        gameObject.SetActive(true);
        bulletFX.Play();
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
            var index = CheckForToleranceHitAboveAndBelowPoint(transform.position, 0.2f);
            //var index = barrier.GetCellFromVector3(transform.position);
            if (index.HasValue)
            {
                MasterAudio.PlaySoundAndForget("Spit-Hits-Barrier");
                barrier.DisableCellsInPlusPattern(index.Value);
                bulletExplosionFX.transform.position = transform.position;
                bulletExplosionFX.Play();
                DisableBullet();
                GameStateMachine.Instance.BriefPauseTime = .05f;
                GameStateMachine.Instance.ChangeTo = States.BRIEF_PAUSE;
                CameraShake.CustomShake(.3f, .3f, 1);
            }
        }
    }

    private int? CheckForToleranceHitAboveAndBelowPoint(Vector3 point, float tolerance )
    {
        var index = barrier.GetCellFromVector3(point);
        if (index.HasValue == false)
        {
            index = barrier.GetCellFromVector3(new Vector3( point.x, point.y + tolerance, point.z));
        }
        if (index.HasValue == false)
        {
            index = barrier.GetCellFromVector3(new Vector3( point.x, point.y + tolerance, point.z));
        }

        return index; 
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