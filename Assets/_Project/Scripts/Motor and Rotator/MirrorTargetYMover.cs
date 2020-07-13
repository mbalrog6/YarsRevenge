using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTargetYMover : MonoBehaviour, IMover
{
    [SerializeField] private Transform _target;
    private Vector3 _position;

    private void Awake()
    {
        _position = transform.position;
    }

    private void Update()
    {
        Tick();
    }

    public void Tick()
    {
        _position.x = transform.position.x;
        _position.y = _target.position.y;
        transform.position = _position;
    }
}
