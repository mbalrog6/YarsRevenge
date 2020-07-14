using UnityEngine;

public class Idle : IState
{
    private Vector3 _position;
    private EntityStateMachine _entity;
    private Transform _transform;
    

    public Idle(EntityStateMachine entity, Vector3 position)
    {
        _entity = entity;
        _transform = _entity.transform;
        _transform.position = position;
        _position = position;
    }

    public void SetPosition(Vector3 position)
    {
        _position = position;
    }
    
    public void Tick()
    {
        Debug.Log("Ticking the Idle state.");
        _transform.position = _position;

    }

    public void OnEnter()
    {
        Debug.Log("Entered the Idle state.");
        _transform.position = _position;
        _entity.SetTimer(3f);
    }

    public void OnExit()
    {
        Debug.Log("Exited the Idle state.");
    }
}