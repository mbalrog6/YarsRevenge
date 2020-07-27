
using UnityEngine;

public class ChargeUp : IState
{
    private readonly EntityStateMachine _entity;
    private Color _color1;
    private Color _color2;
    private float _flickerRate = .1f;
    private float _timer;
    private bool _colorChoice;
    private Material _material;
    private Transform _transform;
    private Transform _targetTransform; 
    
    public ChargeUp(EntityStateMachine entity, Transform target)
    {
        _entity = entity;
        _transform = entity.transform;
        _targetTransform = target;
        _color1 = Color.magenta;
        _color2 = Color.white;
        _timer = Time.time + _timer;
        _colorChoice = true;
        _material = _entity.gameObject.GetComponentInChildren<MeshRenderer>().material; 
    }
    public void Tick()
    {
        if (Time.time > _timer)
        {
            _timer = Time.time + _flickerRate;
            _colorChoice = !_colorChoice;
        }

        _material.color = _colorChoice ? _color1 : _color2;
        _transform.position = _targetTransform.position;
        _entity.WarlordEntity.PlayChargingSound();

    }

    public void OnEnter()
    {
        _entity.SetTimer(2f);
        Warlord.State = WarlordState.ChargeUp;
        _entity.WarlordEntity.PlayChargingSound();
    }

    public void OnExit()
    {
        _entity.WarlordEntity.StopSound();
    }
}