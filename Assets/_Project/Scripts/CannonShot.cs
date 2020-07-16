using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    [SerializeField] private Barrier barrier;
    [SerializeField] private Transform[] contactPoints;

    private RectContainer _collisionRect;
    private MoveToRightScreenMover _mover;
    private MirrorTargetYMover _yMover;
    private HashSet<int> _cellIndexs = new HashSet<int>();

    public bool HasFired { get; private set; }
    public CardinalDirection Direction { get; private set; }
    public Rect Bounds => _collisionRect.Bounds;

    public event Action OnDie; 

    private void Awake()
    {
        _collisionRect = new RectContainer( gameObject, transform.position.x, transform.position.y, .8f, .5f );
        _mover = GetComponentInChildren<MoveToRightScreenMover>();
        _yMover = GetComponentInChildren<MirrorTargetYMover>();
        HasFired = false;
        Direction = CardinalDirection.EAST;
    }

    private void Update()
    {
        _collisionRect.UpdateToTargetPosition();

        if (transform.position.x > ScreenHelper.Instance.ScreenBounds.xMax)
        {
            Die();
        }

        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        if (_collisionRect.Bounds.Overlaps(barrier.BarriorBounds))
        {
            CheckCannonShotContactPointsForBarriorCollision(contactPoints, Vector3.zero);
            if (_cellIndexs.Count > 0)
            {
                foreach (var index in _cellIndexs)
                {
                    barrier.DisableCell(index);
                }

                Die();
            }
        }
    }
    
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
    }

    public void Die()
    {
        _mover.enabled = false;
        _yMover.enabled = true;
        HasFired = false;
        Direction = CardinalDirection.EAST;
        OnDie?.Invoke(); 
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (_collisionRect == null) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
    }
}
