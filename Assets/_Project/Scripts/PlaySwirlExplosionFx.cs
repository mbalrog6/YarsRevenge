using UnityEngine;

public class PlaySwirlExplosionFx : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<ExplosionTransionFinishedCommand>(StartExplosion);
    }

    private void StartExplosion(ExplosionTransionFinishedCommand _explosionCommand)
    {
        if (_particleSystem != null)
        {
            if (_particleSystem.isPlaying == false)
            {
                _particleSystem.Play();
            }
        }
    }
    
}
