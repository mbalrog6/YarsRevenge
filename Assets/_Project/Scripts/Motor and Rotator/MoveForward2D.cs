using System;
using UnityEngine;

public class MoveForward2D : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 _initialFacingVector = Vector3.up;
    
    private Vector3 _direction = Vector3.up;

    private void Awake()
    {
        _direction = _initialFacingVector;
    }

    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE)
        {
            return;
        }
        
        float angle = (transform.eulerAngles.z) * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        _initialFacingVector.x = _direction.x * cos - _direction.y * sin;
        _initialFacingVector.y = _direction.x * sin + _direction.y * cos;

        transform.position += _initialFacingVector * (speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _initialFacingVector );
    }
}
