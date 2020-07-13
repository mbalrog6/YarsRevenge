using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    [SerializeField] private float _rightSideOfScreen;
    private RectContainer _collisionRect;

    private void Awake()
    {
        _collisionRect = new RectContainer( gameObject, transform.position.x, transform.position.y, .8f, .5f );
    }

    private void Update()
    {
        _collisionRect.UpdateToTargetPosition();

        if (transform.position.x > _rightSideOfScreen)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_collisionRect == null) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMin, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3( _collisionRect.Bounds.xMin, _collisionRect.Bounds.yMax, 0f), new Vector3( _collisionRect.Bounds.xMax, _collisionRect.Bounds.yMax, 0f));
    }
}
