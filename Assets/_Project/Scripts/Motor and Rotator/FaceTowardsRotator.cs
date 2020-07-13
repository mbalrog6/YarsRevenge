using System;
using UnityEngine;

public class FaceTowardsRotator : MonoBehaviour, IRotator
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _target;
    private Transform _source;

    private void Awake()
    {
        _source = gameObject.transform;
    }

    private void Update()
    {
        Tick();
    }

    public void Tick()
    {
        var direction =  _target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
}
