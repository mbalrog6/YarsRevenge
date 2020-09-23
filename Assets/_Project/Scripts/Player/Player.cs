using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Barrier2 _barrier;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private CannonShot _cannonShot;
    [SerializeField] private Warlord _warlord;
    [SerializeField] private Probe _probe;
    [SerializeField] private IonZone _ionZone;

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] _frontContactPoints;
    [SerializeField] private Transform[] _backContactPoints;
    [SerializeField] private float eatCooldownTime = 1f;
    [SerializeField] private float _reboundDistance = 0.1f;
    [SerializeField] private float bulletFireRate = .2f;
    [SerializeField] private int cellAmmoValue = 2;
    [SerializeField] private float radius;

    [Header("Audio")] [SerializeField] private SimpleAudioEvent shotSound;
    [SerializeField] private SimpleAudioEvent eatSound;
    [SerializeField] private SimpleAudioEvent dieSound;
    [SerializeField] private SimpleAudioEvent ionCloudAudio;

    [SerializeField] private MeshRenderer PlayerQuad;
    
    public event Action OnDie;

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
    public int Ammo => _ammo;
    private float _suppressShotTimer;

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
    private int _ammo;

    private float _warlordAmmoTimer;
    private bool _playerDead = false;
    private AudioSource _audioSource;
    private AudioSource _buzzSound;
    private AudioSource _ionCloud;


    private void Awake()
    {
        _eatTimer = 0f;
        _startPosition = transform.position;
        
        _mover = new KineticMover(this);
        _rotator = new DirectionalRotator(this);
        
        _canFireBullet = true;
        _fireBulletCooldownTimer = 0f;

        _bullet.OnDisabled += Handle_BulletDisabled;
        _cannonShot.OnDie += Handle_CannonShotOnDie;

        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        _buzzSound = AudioManager.Instance.RequestAudioSource(1);
        _ionCloud = AudioManager.Instance.RequestAudioSource(2);

        _ammo = 0;
        
        _playerRectContainer = new RectContainer(this.gameObject, 10, 10, 1f, 1f);
        _playerCollisions = new PlayerCollisions(_playerRectContainer);
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
    }

    private void Start()
    {
        _playerCollisions.AddEntityRect(EntityCast.Cannon, _cannonShot.CannonRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Warlord, _warlord.WarlordRectContainer);
        _playerCollisions.AddEntityRect(EntityCast.Probe, _probe.ProbeRectContainer);
        
        _bullet.DisableBullet();
        _cannonShot.gameObject.SetActive(false);
        _playerInputDTO = InputManager.Instance.PlayerInputDTO;
    }

    private void Handle_CannonShotOnDie()
    {
        _cannonDeployed = false;
    }

    private void Handle_BulletDisabled()
    {
        _canFireBullet = true;
    }
    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE  || 
            GameStateMachine.Instance.CurrentState == States.BRIEF_PAUSE) 
        {
            return;
        }

        _startPosition = transform.position;
        
        _playerInputDTO = InputManager.Instance.PlayerInputDTO;
        
        _rotator.Tick();

        if (PlayerInputDTO.Direction != CardinalDirection.NONE)
        {
            if(!_buzzSound.isPlaying)
                dieSound.Play(_buzzSound);
        }
        else
        {
            _buzzSound.Stop();
        }

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
        PlayIonSound();
    }

    private void PlayIonSound()
    {
        if (_playerCollisions.CheckIfPlayerHit(_ionZone.IonRectContainer))
        {
            if( !_ionCloud.isPlaying )
                ionCloudAudio.Play(_ionCloud);
        }
        else
        {
            _ionCloud.Stop();
        }
    }

    private void CheckIfPlayerHitProbe()
    {
        if (_probe.IsDead || _playerDead)
            return;

        if (_probe.ProbeRectContainer.Bounds.Overlaps(_ionZone.IonRectContainer.Bounds))
        {
            return;
        }
        
        if (_playerCollisions.CheckPlayerRadiusWithinEntityRadius(EntityCast.Probe, radius, _probe.Radius))
        {
            Die();
        }
    }

    private void ResolvePlayerTurnInBarrierRect()
    {
        if (!_playerCollisions.HasEntityRect(EntityCast.Barrier))
            return; 
        
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Barrier))
        { 
            if (HasChangedFacing())
            {
                BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_backContactPoints, Vector3.zero);
                if (BarrierCellIndex.HasValue)
                {
                    
                    _barrier.SetCellColor(BarrierCellIndex.Value, Color.magenta);
                    var direction = CardinalDirections.GetOppisiteDirection(_playerInputDTO.Direction);
                    if (direction == CardinalDirection.EAST)
                    {
                        direction = CardinalDirection.WEST;
                    } else if (direction == CardinalDirection.NORTH_EAST)
                    {
                        direction = CardinalDirection.NORTH_WEST;
                    } else if (direction == CardinalDirection.SOUTH_EAST)
                    {
                        direction = CardinalDirection.SOUTH_WEST;
                    }
                    var offsetVector = CardinalDirections.GetUnitVectorFromCardinalDirection(direction) * .25f;
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
        if (!_playerCollisions.HasEntityRect(EntityCast.Barrier))
            return; 
        
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Barrier))
        {
            PlayerQuad.material.color = Color.red;
            var offsetVector =
                CardinalDirections.GetUnitVectorFromCardinalDirection(
                    CardinalDirections.GetOppisiteDirection(_playerInputDTO.Direction)) * .2f;
            BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_frontContactPoints, offsetVector);
            if (BarrierCellIndex.HasValue)
            {
                var reboundDistance = _reboundDistance;
                var direction = CardinalDirections.GetOppisiteDirection(_playerInputDTO.LastFacingDirection);
                if (direction == CardinalDirection.EAST)
                {
                    direction = CardinalDirection.WEST;
                    reboundDistance = 0;
                } else if (direction == CardinalDirection.NORTH_EAST)
                {
                    direction = CardinalDirection.NORTH;
                } else if (direction == CardinalDirection.SOUTH_EAST)
                {
                    direction = CardinalDirection.SOUTH;
                }
                
                transform.position = _startPosition + CardinalDirections.GetUnitVectorFromCardinalDirection(direction) * reboundDistance;
                EatCell(BarrierCellIndex.Value);
                _playerRectContainer.UpdateToTargetPosition();
            }
            else
            {
                offsetVector =
                    CardinalDirections.GetUnitVectorFromCardinalDirection(_playerInputDTO.Direction) * .2f;
                BarrierCellIndex = CheckPlayerContactPointsForBarriorCollision(_backContactPoints, offsetVector);
                if (BarrierCellIndex.HasValue)
                {
                    var reboundDistance = _reboundDistance;
                    var direction = _playerInputDTO.LastFacingDirection;
                    if (direction == CardinalDirection.EAST)
                    {
                        direction = CardinalDirection.WEST;
                        reboundDistance = 0;
                    } else if (direction == CardinalDirection.NORTH_EAST)
                    {
                        direction = CardinalDirection.NORTH;
                    } else if (direction == CardinalDirection.SOUTH_EAST)
                    {
                        direction = CardinalDirection.SOUTH;
                    }
                    transform.position = _startPosition +
                                         CardinalDirections.GetUnitVectorFromCardinalDirection(direction) * reboundDistance;
                    _playerRectContainer.UpdateToTargetPosition();
                }
            }
        }
        else
        {
            PlayerQuad.material.color = Color.white;
        }
    }

    private void CheckIfWarlordHitPlayer()
    {
        if (_playerDead == true)
            return; 
        
        if (_playerCollisions.CheckIfPlayerHit(EntityCast.Warlord))
        {
            switch (Warlord.State)
            {
                case WarlordState.Idle:
                    if (_warlord.CanGetAmmo)
                    {
                        _ammo += _warlord.GetAmmo();
                    }
                    break;
                case WarlordState.ChargeUp:
                    Die();
                    break;
                case WarlordState.LaunchedTowardsPlayer:
                    Die();
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
            if (_cannonShot.Direction == CardinalDirection.EAST)
            {
                Die();
            }
            else
            {
                _ammo += 10; 
            }
            _cannonShot.Die();
        }
    }

    private void FireCannon()
    {
        if (_playerInputDTO.FireButton && !_cannonShot.HasFired)
        {
            _cannonShot.Fire();
        }
    }

    private void DeployCannonIFPossible()
    {
        if (!_cannonDeployed && transform.position.x <= ScreenHelper.Instance.ScreenBounds.xMin && _ammo >= 10)
        {
            _cannonDeployed = true;
            _ammo -= 10;
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
        if (_fireBulletCooldownTimer > Time.time || _ammo <= 0 )
            return;

        if (!_bullet.isActiveAndEnabled && 
            _playerInputDTO.FireButton == true && 
            _canFireBullet &&
            _playerInputDTO.LastFacingDirection != CardinalDirection.NONE)
        {
            _bullet.FireBullet();
            _canFireBullet = false;
            _fireBulletCooldownTimer = Time.time + bulletFireRate;
            _ammo -= 1;
            
            shotSound.PlayOneShot(_audioSource);
        }
    }

    private void EatCell(int cellIndex)
    {
        if (Time.time >= _eatTimer)
        {
            _barrier.DisableCell(cellIndex);
            _eatTimer = Time.time + eatCooldownTime;
            _ammo += cellAmmoValue;
            GameManager.Instance.AddScore(_barrier.Score(cellIndex));
        }
        eatSound.Play(_audioSource);
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

    void Die()
    {
        Debug.Log("Player Died");
        _playerDead = true; 
        _probe.Die();
        GameManager.Instance.KillPlayer();
        OnDie?.Invoke();
        GetComponentInChildren<MeshRenderer>().enabled = false;
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(.3f);
        Debug.Log("RESPAWN SET");
        var yOffset = ScreenHelper.Instance.ScreenBounds.yMax - 2f;
        var x = ScreenHelper.Instance.ScreenBounds.xMin + 2f;
        transform.position = new Vector3(x, UnityEngine.Random.Range(-yOffset, yOffset), 0f);
        _playerDead = false; 
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    private void UpdateBarrier( Barrier2 barrier)
    {
        _barrier = barrier;
        ChangeBarrierRef(_barrier);
    }

    private bool HasChangedFacing()
    {
        if (_playerInputDTO.Direction != _playerInputDTO.LastFacingDirection)
        {
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _playerDead = false; 
        _ammo = 0;
         GetComponentInChildren<MeshRenderer>().enabled = false;
         StartCoroutine(Respawn());
         _bullet.Reset();
         _cannonDeployed = false;
         _cannonShot.Reset();
    }

    public void ChangeBarrierRef(Barrier2 barrier)
    {
        _playerCollisions.RemoveEntityRect(EntityCast.Barrier);
        _playerCollisions.AddEntityRect(EntityCast.Barrier, _barrier.BarrierRectContainer);
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