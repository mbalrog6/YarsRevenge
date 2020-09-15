using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    [SerializeField] private Barrier2 barrier;
    [SerializeField] private Probe probe;
    [SerializeField] private Warlord qotile;
    [SerializeField] private Transform[] contactPoints;

    [Header("Audio")] [SerializeField] private SimpleAudioEvent explosion;
    [SerializeField] private SimpleAudioEvent cannonFire;
    [SerializeField] private SimpleAudioEvent rebound;
    private AudioSource _audioSource;

    private RectContainer _collisionRect;
    private DirectionalMover _mover;
    private MirrorTargetYMover _yMover;
    private HashSet<int> _cellIndexs = new HashSet<int>();

    public bool HasFired { get; private set; }
    public CardinalDirection Direction { get; private set; }
    public Rect Bounds => _collisionRect.Bounds;
    public RectContainer CannonRectContainer => _collisionRect;

    public event Action OnDie;

    private void Awake()
    {
        _collisionRect = new RectContainer(gameObject, transform.position.x, transform.position.y, .8f, .5f);
        _mover = GetComponentInChildren<DirectionalMover>();
        _yMover = GetComponentInChildren<MirrorTargetYMover>();
        HasFired = false;
        Direction = CardinalDirection.EAST;
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
    }

    private void Update()
    {
        _collisionRect.UpdateToTargetPosition();

        CheckForCollisions();
        CheckOffScreen();
    }

    private void CheckOffScreen()
    {
        if (!HasFired)
            return;

        if (!ScreenHelper.Instance.ScreenBounds.Contains(transform.position))
        {
            Die();
        }
    }

    #region Collisions...
    private void CheckForCollisions()
    {
        CheckForBarrierCollision();
        CheckForProbeCollision();

        if (this.HasFired)
        {
            CheckForQotileCollision();
        }
    }

    private void CheckForQotileCollision()
    {
        if (_collisionRect.Bounds.Overlaps(qotile.WarlordRectContainer.Bounds))
        {
            GameManager.Instance.AddScore(qotile.Score);
            Explode();
            qotile.Die();
        }
    }

    private void CheckForProbeCollision()
    {
        if (!probe.IsDead)
        {
            if (_collisionRect.Bounds.Overlaps(probe.ProbeRectContainer.Bounds))
            {
                Explode();
                probe.Die();
                GameManager.Instance.AddScore(probe.Score);
            }
        }
    }

    private void CheckForBarrierCollision()
    {
        if (_collisionRect.Bounds.Overlaps(barrier.BarrierRectContainer.Bounds) && Direction == CardinalDirection.EAST)
        {
            CheckCannonShotContactPointsForBarriorCollision(contactPoints, Vector3.zero);
            if (_cellIndexs.Count > 0)
            {
                foreach (var index in _cellIndexs)
                {
                    GameManager.Instance.AddScore(barrier.Score(index));
                    barrier.DisableCell(index);
                }

                if (barrier.BarrierComponent.BarrierInfo.IsReflective)
                {
                    Direction = CardinalDirection.WEST;
                    _mover.Direction = Direction;
                    rebound.PlayOneShot(_audioSource);
                }
                else
                {
                    Explode();
                }
            }
        }
    }
    #endregion

    private void CheckCannonShotContactPointsForBarriorCollision(Transform[] contactPoints, Vector3 offset)
    {
        _cellIndexs.Clear();
        int? hit = null;
        foreach (var point in contactPoints)
        {
            if (Vector3.zero != offset)
            {
                hit = barrier.GetCellFromVector3(point.position + offset);
                if (hit.HasValue)
                {
                    _cellIndexs.Add(hit.Value);
                }
            }

            hit = barrier.GetCellFromVector3(point.position);
            if (hit.HasValue)
            {
                _cellIndexs.Add(hit.Value);
            }
        }
    }

    public void EnableMover()
    {
        _mover.enabled = true;
        _yMover.enabled = false;
        HasFired = true;
        Direction = CardinalDirection.EAST;
        _mover.Direction = CardinalDirection.EAST;
    }

    public void Die()
    {
        Reset();
        OnDie?.Invoke();
    }

    public void Reset()
    {
        _mover.enabled = false;
        _yMover.enabled = true;
        HasFired = false;
        Direction = CardinalDirection.EAST;
        _mover.Direction = CardinalDirection.EAST;
        gameObject.SetActive(false);
    }

    public void Fire()
    {
        EnableMover();
        cannonFire.PlayOneShot(_audioSource);
        HasFired = true;
    }

    public void Explode()
    {
        GameStateMachine.Instance.BriefPauseTime = .3f;
        GameStateMachine.Instance.ChangeTo = States.BRIEF_PAUSE;
        explosion.PlayOneShot(_audioSource);
        Die();
    }

    private void UpdateBarrier(Barrier2 barrier)
    {
        this.barrier = barrier;
    }

    private void OnDrawGizmos()
    {
        if (_collisionRect == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f),
            new Vector3(_collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f),
            new Vector3(_collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f),
            new Vector3(_collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f),
            new Vector3(_collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
    }
}