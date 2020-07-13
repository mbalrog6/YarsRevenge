using UnityEngine;

public class KineticMover : IMover
{
    private Transform _transform;
    private Player _player;
    private Vector3 _moveVector;

    public KineticMover(Player player)
    {
        _player = player;
        _transform = player.transform;
    }
    
    public void Tick()
    {
        _moveVector = _player.DirectionVector;
        _moveVector *= (Time.deltaTime * _player.MovementSpeed);
        
        _transform.position += _moveVector;
    }
}