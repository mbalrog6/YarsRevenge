using UnityEngine;

public class Idle : IState
{
    private Barrier2 _barrier;
    private EntityStateMachine _entity;
    private Transform _transform;

    public Idle(EntityStateMachine entity)
    {
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
        _entity = entity;
        _transform = _entity.transform;
        if (_barrier != null)
        {
            _transform.position = _barrier.WarlordSpawnPoint;
        }
    }
    public void Tick()
    {
        _transform.position = _barrier.WarlordSpawnPoint;
    }

    public void OnEnter()
    {
        if (_barrier != null)
        {
            _transform.position = _barrier.WarlordSpawnPoint;
        }
        _entity.SetTimer(3f);
        Warlord.State = WarlordState.Idle;
    }

    public void OnExit()
    {
        
    }

    public void UpdateBarrier(Barrier2 barrier)
    {
        _barrier = barrier;
    }
}