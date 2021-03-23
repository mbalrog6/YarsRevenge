using UnityEngine;

public class ProbeExplosion : MonoBehaviour
{
    [SerializeField] private Probe _probe;

    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _probe.OnDie += Explode;

        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        _probe.OnDie -= Explode;
    }

    private void Explode()
    {
        transform.position = _probe.transform.position;
        _particleSystem.Play();
        CameraShake.CustomShake(.2f, .2f, 0);
    }
}
