using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLeftScreenMover : MonoBehaviour, IMover
{
    [SerializeField] private float _speed; 
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
        _position.y = transform.position.y;
        _position.x += _speed * Time.deltaTime;
        transform.position = _position;
    }
}
