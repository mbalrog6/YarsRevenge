using System;
using UnityEngine;

public class DirectionalMover : MonoBehaviour, IMover
{
    [SerializeField] private Transform _entity;
    [SerializeField] private float _speed;
    [SerializeField] private CardinalDirection _direction;

    public CardinalDirection Direction
    {
        get => _direction;
        set => _direction = value;
    }

    public float Speed => _speed; 
    private Vector3 _directionVector;
   

    public void Tick()
    {
        Move();
    }
    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (GameStateMachine.Instance.CurrentState == States.BRIEF_PAUSE ||
            GameStateMachine.Instance.CurrentState == States.PAUSE)
        {
            return;
        }
        
        _directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(Direction);
        _entity.position += _directionVector * (Time.deltaTime * Speed);
    }
}
