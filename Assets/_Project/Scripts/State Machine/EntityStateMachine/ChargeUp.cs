
using UnityEngine;

public class ChargeUp : IState
{
    private QotileAnimationController qotileAnimationController;
    private bool _swirlAnimationActivated;
    
    private readonly EntityStateMachine _entity;
    private float _flickerRate = .9f;
    private float _timer;
    private bool _colorChoice;
    private Transform _transform;
    private Barrier2 _barrier;
    private float chargeTime;
    private float chargeTimeMin;
    private float chargeTimeMax;

    public ChargeUp(EntityStateMachine entity)
    {
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
        _entity = entity;
        _transform = entity.transform;
        _colorChoice = true;
    }
    public void Tick()
    {
        if (Time.time > _timer)
        {
            _timer = Time.time + _flickerRate;
            _entity.QotileEntity.ActivateSwirlAnimation();
            _swirlAnimationActivated = true;
        }
        
        _transform.position = _barrier.WarlordSpawnPoint;
        _entity.QotileEntity.PlayChargingSound();

    }

    public void OnEnter()
    {
        _swirlAnimationActivated = false;
        _timer = Time.time + _flickerRate;
        qotileAnimationController = QotileAnimationController.Instance;
        _entity.SetTimer(_entity.ChargeTime);
        Warlord.State = WarlordState.ChargeUp;
        _entity.QotileEntity.PlayChargingSound();
        if (qotileAnimationController != null)
        {
            qotileAnimationController.TriggerOpenGate();
        }
    }

    public void OnExit()
    {
        if (_swirlAnimationActivated == false)
        {
            _entity.QotileEntity.ActivateSwirlAnimation();
        }
        _entity.QotileEntity.StopSound();
        _entity.ChargeTimeAdvancementCount++;
        if (qotileAnimationController != null)
        {
            qotileAnimationController.TriggerCloseGate();
        }
    }
    
    public void UpdateBarrier(Barrier2 barrier)
    {
        _barrier = barrier;
    }
}