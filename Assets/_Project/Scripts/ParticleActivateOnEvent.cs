using System;
using UnityEngine;

public class ParticleActivateOnEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private IFinishedEvent _isFinishedEvent;

    private void Awake()
    {
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        if (_particleSystem == null)
        {
            throw new NullReferenceException("No Particle System is associated with this GameObject");
        }

        _isFinishedEvent = GetComponentInParent<IFinishedEvent>();
    }

    public void Start()
    {
        if (_isFinishedEvent != null)
        {
            _isFinishedEvent.IsFinished += Activate;
        }
    }

    public void Activate()
    {
        _particleSystem.Play();
    }
}
