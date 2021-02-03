using UnityEngine;

public class MoveToRightScreenMover : MonoBehaviour, IMover
{
    [SerializeField] private float _speed;
    public bool Paused { get; set; }
    private Vector3 _position;

    private void Awake()
    {
        _position = transform.position;
    }

    private void Update()
    {
        if (Paused)
            return; 
        Tick();
    }

    public void Tick()
    {
        _position = transform.position;
        _position.x += _speed * Time.deltaTime;
        transform.position = _position;
    }
}
