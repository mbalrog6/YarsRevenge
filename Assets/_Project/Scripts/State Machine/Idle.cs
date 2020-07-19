using UnityEngine;

public class Idle : IState
{
    private Transform _target;
    private EntityStateMachine _entity;
    private Transform _transform;
    

    public Idle(EntityStateMachine entity, Transform target)
    {
        _entity = entity;
        _transform = _entity.transform;
        _transform.position = target.position;
        _target = target;
    }
    public void Tick()
    {
        Debug.Log("Ticking the Idle state.");
        _transform.position = _target.position;

    }

    public void OnEnter()
    {
        Debug.Log("Entered the Idle state.");
        _transform.position = _target.position;
        _entity.SetTimer(3f);
        Warlord.State = WarlordState.Idle;
    }

    public void OnExit()
    {
        Debug.Log("Exited the Idle state.");
    }
}