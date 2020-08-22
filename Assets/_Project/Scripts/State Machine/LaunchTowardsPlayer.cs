using UnityEngine;

public class LaunchTowardsPlayer : IState
{
    private readonly EntityStateMachine _entity;
    private readonly Player _player;
    private Transform _playerTransform;
    private Vector3 _luanchDirection;
    private float _speed =10f;

    public LaunchTowardsPlayer(EntityStateMachine entity, Player player)
    {
        _entity = entity;
        if (player == null)
        {
            Debug.Log("Player is null", _entity.gameObject);
        }
        else
        {
            _playerTransform = player.transform;
        }
        
        _player = player;
        
    }
    public void Tick()
    {
        if (_player == null)
            return;
        
        _entity.transform.position += _luanchDirection * (_speed * Time.deltaTime);
        _entity.WarlordEntity.PlayLaunchAtSound();
    }

    public void OnEnter()
    {
        //_entity.SetTimer(3f);
       
        if (_player == null)
            return;
        _speed = _entity.LaunchSpeed;
        var playerPosition = _playerTransform.position;
        _luanchDirection = playerPosition - _entity.transform.position;
        _luanchDirection.Normalize();
        
        Warlord.State = WarlordState.LaunchedTowardsPlayer;
        _entity.WarlordEntity.PlayLaunchAtSound();
    }

    public void OnExit()
    {
        _entity.WarlordEntity.StopSound();
        _entity.LaunchSpeedAdvancementCount++;
    }
}