using UnityEngine;

public class Idle : IState
{
    private Transform _position;
    private EntityStateMachine _entity;
    private Transform _transform;
    

    public Idle(EntityStateMachine entity, Transform position)
    {
        _entity = entity;
        _transform = _entity.transform;
        _transform.position = position.position;
        _position = position;
    }
    public void Tick()
    {
        Debug.Log("Ticking the Idle state.");
        _transform.position = _position.position;

    }

    public void OnEnter()
    {
        Debug.Log("Entered the Idle state.");
        _transform.position = _position.position;
        _entity.SetTimer(3f);
        Warlord.State = WarlordState.Idle;
    }

    public void OnExit()
    {
        Debug.Log("Exited the Idle state.");
    }
}