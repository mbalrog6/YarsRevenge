using System;
using System.Collections.Generic;
using UnityEngine;
using YarsRevenge._Project.Audio;
using YarsRevenge._Project.Scripts.Audio.Audio_Scripts;

public class CannonShot : MonoBehaviour
{
    [SerializeField] private Barrier2 barrier;
    [SerializeField] private Probe probe;
    [SerializeField] private Warlord qotile;
    [SerializeField] private Transform[] contactPoints;

    [Header("Audio")]
    [SerializeField] private PlaySound explosion;
    [SerializeField] private PlaySound cannonFire;
    [SerializeField] private PlaySound rebound;
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
    public event Action OnExplode;

    private void Awake()
    {
        _collisionRect = new RectContainer(gameObject, transform.position.x, transform.position.y, .8f, .5f);
        _mover = GetComponentInChildren<DirectionalMover>();
        _yMover = GetComponentInChildren<MirrorTargetYMover>();
        HasFired = false;
        Direction = CardinalDirection.EAST;
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;

        if (cannonFire == null)
        {
            cannonFire = ScriptableObject.CreateInstance<MockSimpleAudioEvent>();
        }

        if (rebound == null)
        {
            rebound = ScriptableObject.CreateInstance<MockSimpleAudioEvent>();
        }

        if (explosion == null)
        {
            explosion = ScriptableObject.CreateInstance<MockSimpleAudioEvent>();
        }
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
                    barrier.DisableCellsIn3x3Pattern(index);
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
                    Die();
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
        OnDie?.Invoke();
        Reset();
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
        OnExplode?.Invoke();
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