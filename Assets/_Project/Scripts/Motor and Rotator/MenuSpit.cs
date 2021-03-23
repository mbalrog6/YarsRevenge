using System;
using UnityEngine;

public class MenuSpit : MonoBehaviour
{
 [SerializeField] private bool playOnce;
 [SerializeField] private AnimationCurve travelEasing;
 [SerializeField] private AnimationCurve scaleEasing;

 [SerializeField] private Transform startPosition;
 [SerializeField] private Transform endPosition;

 [SerializeField] private float startScale;
 [SerializeField] private float endScale;

 [SerializeField] private float timeToTravel;

 public event Action OnReachedDestination;

 private ParticleSystem _particleSystem;

 private Transform myTransform;
 private Vector3 _startingLocalScale;
 private Vector3 _positionVector;
 private float _distanceToTravel;
 private float _time;

 private void Awake()
 {
  myTransform = GetComponent<Transform>();
  _particleSystem = GetComponent<ParticleSystem>();
  _startingLocalScale = myTransform.localScale;
  _time = 0;
  _positionVector = endPosition.position - startPosition.position;
  _distanceToTravel = _positionVector.magnitude;
  _positionVector.Normalize();
 }

 private void Update()
 {
  if (_time > timeToTravel)
  {
   OnReachedDestination?.Invoke();
   if (playOnce)
   {
    if (_particleSystem != null)
    {
     _particleSystem.Stop();
    }
    return;
   }
   _time = 0;
   ResetPosition();
  }

  _time += Time.deltaTime;
  var distance = _positionVector * (travelEasing.Evaluate(_time) * _distanceToTravel);
  var position = startPosition.position + distance;
  myTransform.position = position;

  var scalePercentageByDistance = 1f - ((_distanceToTravel - distance.magnitude)/_distanceToTravel);
  myTransform.localScale = _startingLocalScale * (scaleEasing.Evaluate(scalePercentageByDistance) * endScale + startScale);
 }

 private void ResetPosition()
 {
  if (_particleSystem != null)
  {
   _particleSystem.Stop();
  }
  myTransform.position = startPosition.position;
  if (_particleSystem != null)
  {
   _particleSystem.Play();
  }
 }
}
