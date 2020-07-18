using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Barrier _barrier;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private CannonShot _cannonShot;
    [SerializeField] private Warlord _warlord;
    [SerializeField] private Probe _probe; 

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] _frontContactPoints;
    [SerializeField] private Transform[] _backContactPoints;
    [SerializeField] private float eatCooldownTime = 1f;
    [SerializeField] private float _reboundDistance = 0.1f;
    [SerializeField] private float bulletFireRate = .2f;
    [SerializeField] private int cellAmmoValue = 2;
    [SerializeField] private float radius;

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
    private InputDTO _playerInputDTO;
    private IMover _mover;
    private IRotator _rotator;
    private RectContainer _playerRectContainer;
    private PlayerCollisions _playerCollisions;
    private float _eatTimer;
    private int? BarrierCellIndex;
    private bool _canFireBullet;
    private float _fireBulletCooldownTimer;
    private bool _cannonDeployed;
    private int ammo = 0;
    
    private float delayTimer = 0f;
    private float _warlordAmmoTimer;


    private void Awake()
    {
        _eatTimer = 0f;
        _startPosition = transform.position;
        PlayerInput = new PlayerInput();
        _mover = new KineticMover(this);
        _rotator = new DirectionalRotator(this);
        
        _canFireBullet = true;
        _fireBulletCooldownTimer = 0f;

        _bullet.OnDisabled += Handle_BulletDisabled;
        _cannonShot.OnDie += Handle_CannonShotOnDie;
    }

    private void Start()
    {
        _playerCollisions = new PlayerCollisions(_playerRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Cannon, _cannonShot.CannonRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Barrier, _barrier.BarrierRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Warlord, _warlord.WarlordRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Probe, _probe.ProbeRectContainer);
        
        _bullet.DisableBullet();
        _cannonShot.gameObject.SetActive(false);
    }

    private void Handle_CannonShotOnDie()
    {
        _cannonDeployed = false;
    }

    private void Handle_BulletDisabled()
    {
        _canFireBullet = true;
    }

    private void OnValidate()
    {
        _playerRectContainer = new RectContainer(this.gameObject, 10, 10, 1f, 1f);
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

        BarrierCellIndex = null;

        ResolvePlayerTurnInBarrierRect();

        if (!BarrierCellIndex.HasValue)
        {
            _mover.Tick();
            _playerRectContainer.UpdateToTargetPosition();
        }
        
        DidPlayerHitBarrierCell();
        CheckIfWarlordHitPlayer();
        CheckIfCannonHitPlayer();
        DeployCannonIFPossible();

        LimitMovementToRightAndLeftScreenEdge();
        WrapAroundTopBottomOfScreen();

        if (_cannonDeployed)
        {
            FireCannon();
        }
        else
        {
            FireBulletIfPossible();
        }

        CheckIfPlayerHitProbe();
    }

    private void CheckIfPlayerHitProbe()
    {
        if (_probe.IsDead)
            return;
        
        if (_playerCollisions.CheckPlayerRadiusWithinEntityRadius(EntityCast.Probe, radius, _probe.Radius))
        {
            _probe.Die();
        }
    }

    private void ResolvePlayerTurnInBarrierRect()
    {
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Barrier))
        {
            if (HasChangedFacing())
            {
                BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_backContactPoints, Vector3.zero);
                if (BarrierCellIndex.HasValue)
                {
                    
                    _barrier.SetCellColor(BarrierCellIndex.Value, Color.magenta);
                    var offsetVector =
                        CardinalDirections.GetUnitVectorFromCardinalDirection(
                            CardinalDirections.GetOppisiteDirection(_playerInputDTO.Direction)) * .25f;
                    BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_frontContactPoints, offsetVector);
                    if (BarrierCellIndex.HasValue)
                    {
                        _playerInputDTO.Direction = _playerInputDTO.LastFacingDirection;
                        PlayerInput.SetInput(_playerInputDTO);
                        _rotator.Tick();
                        _playerRectContainer.UpdateToTargetPosition();
                    }
                }
            }
        }
    }

    private void DidPlayerHitBarrierCell()
    {
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Barrier))
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
                _playerRectContainer.UpdateToTargetPosition();
            }
        }
    }

    private void CheckIfWarlordHitPlayer()
    {
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Warlord))
        {
            switch (Warlord.State)
            {
                case WarlordState.Idle:
                    if (_warlord.CanGetAmmo)
                    {
                        ammo += _warlord.GetAmmo();
                    }
                    break;
                case WarlordState.ChargeUp:
                    Debug.Log("Player Killed by Charging Warlord");
                    break;
                case WarlordState.LaunchedTowardsPlayer:
                    Debug.Log("Warlord Hit Player");
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckIfCannonHitPlayer()
    {
        if (_cannonShot.HasFired && _playerCollisions.CheckIfPlayerHit(EntityCast.Cannon))    
        {
            Debug.Log("Cannon Hit Player");
            _cannonShot.Die();
        }
    }

    private void FireCannon()
    {
        if (PlayerInput.Inputs.FireButton)
        {
            _cannonShot.EnableMover();
        }
    }

    private void DeployCannonIFPossible()
    {
        if (!_cannonDeployed && transform.position.x <= ScreenHelper.Instance.ScreenBounds.xMin && ammo >= 10)
        {
            _cannonDeployed = true;
            ammo -= 10;
            _cannonShot.gameObject.SetActive(true);
            _cannonShot.transform.position =
                new Vector3(ScreenHelper.Instance.ScreenBounds.xMin + .5f, transform.position.y, 0f);
        }
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
        if (_fireBulletCooldownTimer > Time.time || ammo <= 0)
            return;

        if (!_bullet.isActiveAndEnabled && PlayerInput.Inputs.FireButton == true && _canFireBullet)
        {
            _bullet.FireBullet();
            _canFireBullet = false;
            _fireBulletCooldownTimer = Time.time + bulletFireRate;
            ammo -= 1;
        }
    }

    private void EatCell(int cellIndex)
    {
        if (Time.time >= _eatTimer)
        {
            _barrier.DisableCell(cellIndex);
            _eatTimer = Time.time + eatCooldownTime;
            ammo += cellAmmoValue;
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
        if (_playerRectContainer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_playerRectContainer.Bounds.xMin, _playerRectContainer.Bounds.yMin, 0f),
            new Vector3(_playerRectContainer.Bounds.xMin, _playerRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_playerRectContainer.Bounds.xMax, _playerRectContainer.Bounds.yMin, 0f),
            new Vector3(_playerRectContainer.Bounds.xMax, _playerRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_playerRectContainer.Bounds.xMin, _playerRectContainer.Bounds.yMin, 0f),
            new Vector3(_playerRectContainer.Bounds.xMax, _playerRectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_playerRectContainer.Bounds.xMin, _playerRectContainer.Bounds.yMax, 0f),
            new Vector3(_playerRectContainer.Bounds.xMax, _playerRectContainer.Bounds.yMax, 0f));
        
        Gizmos.DrawWireSphere( transform.position, radius);
    }
}