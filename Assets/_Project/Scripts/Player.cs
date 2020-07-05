using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Barrier _barrier; 
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] _contactPoints;

    public Vector3 DirectionVector => CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInput.Inputs.Direction);
    public Vector3 LastFacingDirectionVector => CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInput.Inputs.LastFacingDirection);
    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    private Vector3 _startPosition;
    private IMover _mover;
    private IRotator _rotator;
    private RectContainer _collisionRect;
    public IPlayerInput PlayerInput { get; set; }

    private void Awake()
    {
        _startPosition = transform.position;
        PlayerInput = new PlayerInput();
        _mover = new KineticMover(this);
        _rotator = new DirectionalRotator(this);
       
    }

    private void OnValidate()
    {
        _collisionRect = new RectContainer(this.gameObject, 10, 10, 1f,1f);
    }

    private void Update()
    {
        _startPosition = transform.position;
        PlayerInput.Tick();
        _rotator.Tick();
        _mover.Tick();
        _collisionRect.UpdateToTargetPosition();

        if (CheckForRectCollision(_collisionRect.Bounds, _barrier.BarriorBounds))
        {
            // var hit = _barrier.GetCellFromVector3(transform.position);
            // if (hit.HasValue)
            // {
            //     _barrier.SetCellColor(hit.Value, Color.blue);
            //     transform.position = _startPosition;
            // }
            CheckForPointCollision();
        }
    }

    private bool CheckForRectCollision(Rect originator, Rect targetObject)
    {
        return originator.Overlaps(targetObject);
    }

    private void CheckForPointCollision()
    {
        int? hit;
        foreach (var point in _contactPoints)
        {
            hit = _barrier.GetCellFromVector3(point.position);
            if (hit.HasValue)
            {
                _barrier.SetCellColor(hit.Value, Color.blue);
                transform.position = _startPosition + (CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInput.Inputs.Direction) * -.01f);
                break;
            }
        }
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