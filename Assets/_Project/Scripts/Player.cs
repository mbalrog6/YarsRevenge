using System.Collections;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Barrier _barrier;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] _frontContactPoints;
    [SerializeField] private Transform[] _backContactPoints;
    [SerializeField] private float eatCooldownTime = 1f;
    [SerializeField] private float _reboundDistance = 0.1f;


    public Vector3 DirectionVector =>
        CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInputDTO.Direction);

    public Vector3 LastFacingDirectionVector =>
        CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInputDTO.LastFacingDirection);

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public IPlayerInput PlayerInput { get; set; }
    public InputDTO PlayerInputDTO => _playerInputDTO;

    private Vector3 _startPosition;
    private IMover _mover;
    private IRotator _rotator;
    private RectContainer _collisionRect;
    private float _eatTimer;
    private int? BarrierCellIndex;
    private InputDTO _playerInputDTO;
    private float delayTimer = 0f;
    private bool _canFireBullet;

    private void Awake()
    {
        _eatTimer = 0f;
        _startPosition = transform.position;
        PlayerInput = new PlayerInput();
        _mover = new KineticMover(this);
        _rotator = new DirectionalRotator(this);
        
        _bullet.DisableBullet();
        _canFireBullet = true;
    }

    private void OnValidate()
    {
        _collisionRect = new RectContainer(this.gameObject, 10, 10, 1f, 1f);
    }

    private void Update()
    {
        if (Time.time < delayTimer)
        {
            return;
        }
        delayTimer = Time.time + 0.0f; 
        
        _startPosition = transform.position;

        PlayerInput.Tick();
        PlayerInput.CopyDTO(ref _playerInputDTO);
        _rotator.Tick();

        LimitMovementToRightAndLeftScreenEdge();
        WrapAroundTopBottomOfScreen();

        BarrierCellIndex = null; 

        if (CheckForRectCollision(_collisionRect.Bounds, _barrier.BarriorBounds))
        {
            if (HasChangedFacing())
            {
                BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_backContactPoints, Vector3.zero);
                if (BarrierCellIndex.HasValue)
                {
                    _barrier.SetCellColor(BarrierCellIndex.Value, Color.magenta);
                    _playerInputDTO.Direction = _playerInputDTO.LastFacingDirection;
                    PlayerInput.SetInput(_playerInputDTO);
                    _rotator.Tick();
                    _collisionRect.UpdateToTargetPosition();
                }
            }
        }

        if (!BarrierCellIndex.HasValue)
        {
            _mover.Tick();
            _collisionRect.UpdateToTargetPosition();
        }
        
        DebugText.Instance.SetText(_playerInputDTO.ToString() + "\r\nPreviousLastFacing = ");
        if (CheckForRectCollision(_collisionRect.Bounds, _barrier.BarriorBounds))
        {

            var offsetVector =
                CardinalDirections.GetUnitVectorFromCardinalDirection(
                    CardinalDirections.GetOppisiteDirection(_playerInputDTO.Direction)) * .25f;
            BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_frontContactPoints, offsetVector);
            if (BarrierCellIndex.HasValue)
            {
                transform.position = _startPosition +
                                     (CardinalDirections.GetUnitVectorFromCardinalDirection(_playerInputDTO.Direction) *
                                      -_reboundDistance);

                EatCell(BarrierCellIndex.Value);
                _collisionRect.UpdateToTargetPosition();
            }
        }

        FireBulletIfPossible();
    }

    private void WrapAroundTopBottomOfScreen()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        
        if (y > ScreenHelper.Instance.ScreenBounds.yMax)
        {
            y = ScreenHelper.Instance.ScreenBounds.yMin;
        }
        
        if (y < ScreenHelper.Instance.ScreenBounds.yMin)
        {
            y = ScreenHelper.Instance.ScreenBounds.yMax;
        }
        
        transform.position = new Vector3(x, y, 0f);
    }

    private void LimitMovementToRightAndLeftScreenEdge()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        
        if (x > ScreenHelper.Instance.ScreenBounds.xMax)
        {
            x = ScreenHelper.Instance.ScreenBounds.xMax;
        }
        
        if (x < ScreenHelper.Instance.ScreenBounds.xMin)
        {
            x = ScreenHelper.Instance.ScreenBounds.xMin;
        }

        transform.position = new Vector3(x, y, 0f);
    }
    
    

    private void FireBulletIfPossible()
    {
        if (!_bullet.isActiveAndEnabled && PlayerInput.Inputs.FireButton == true)
        {
            _bullet.FireBullet();
        }
        
    }

    private void EatCell(int cellIndex)
    {
        if (Time.time >= _eatTimer)
        {
            _barrier.DisableCell(cellIndex);
            _eatTimer = Time.time + eatCooldownTime;
        }
    }

    private int? CheckPlayerContactPointsForBarriorCollision(Transform[] contactPoints, Vector3 offset)
    {
        int? hit = null;
        foreach (var point in contactPoints)
        {
            if (Vector3.zero != offset)
            {
                hit = _barrier.GetCellFromVector3(point.position + offset);
                if (hit.HasValue)
                {
                    return hit;
                }
            }

            hit = _barrier.GetCellFromVector3(point.position);
            if (hit.HasValue)
            {
                return hit;
            }
        }

        return hit;
    }

    private bool CheckForRectCollision(Rect originator, Rect targetObject)
    {
        return originator.Overlaps(targetObject);
    }
    
    private bool HasChangedFacing()
    {
        if (_playerInputDTO.Direction != _playerInputDTO.LastFacingDirection)
        {
            return true;
        }

        return false;
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