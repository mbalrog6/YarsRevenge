using System;
using UnityEngine;

public class CannonShotExplosion : MonoBehaviour
{
  [SerializeField] private CannonShot _cannonShot;

  private ParticleSystem _particleSystem;
  private void Awake()
  {
    _cannonShot.OnExplode += Explode;

    _particleSystem = GetComponent<ParticleSystem>();
  }

  private void Explode()
  {
    transform.position = _cannonShot.transform.position;
    _particleSystem.Play();
    CameraShake.CustomShake(.4f, .8f, 0, 10);
  }
}
