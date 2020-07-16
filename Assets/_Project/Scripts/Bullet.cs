using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Player player;
    [SerializeField] private float radius = .2f;
    [SerializeField] private Barrier barrier;

    private Vector3 _fireDirection;
    
    public bool HasBeenFired { get; private set; }

    public event Action OnDisabled;

    public void DisableBullet()
    {
        HasBeenFired = false;
        gameObject.SetActive(false);
        OnDisabled?.Invoke();
    }

    public void FireBullet()
    {
        gameObject.SetActive(true);
        HasBeenFired = true;
        _fireDirection = CardinalDirections.GetUnitVectorFromCardinalDirection(player.PlayerInputDTO.LastFacingDirection);
        transform.position = spawnPoint.position;
    }

    private void Update()
    {
        if (HasBeenFired)
        {
            Move();
            CheckForCollisionWithBarrior();
            CheckForOffScreen();
        }
    }

    private void CheckForOffScreen()
    {
        if (!ScreenHelper.Instance.ScreenBounds.Contains(transform.position))
        {
            DisableBullet();
        }
    }

    private void CheckForCollisionWithBarrior()
    {
        var collision = barrier.BarriorBounds.Contains(transform.position);
        if (collision)
        {
            var index = barrier.GetCellFromVector3(transform.position);
            if (index.HasValue)
            {
                barrier.DisableCellsInPlusPattern(index.Value);
                DisableBullet();
            }
        }
    }

    private void Move()
    {
        transform.position += _fireDirection * (speed * Time.deltaTime);
    }
}