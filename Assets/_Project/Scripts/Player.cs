using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    public Vector3 DirectionVector => GetDirectionUnitVector(PlayerInput.PlayerDirection);
    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    private IMover _mover;
    public IPlayerInput PlayerInput { get; set; }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        _mover = new KineticMover(this);
    }

    private void Update()
    {
        PlayerInput.Tick();
        _mover.Tick();
    }

    private Vector3 GetDirectionUnitVector(Direction direction)
    {
        Vector3 vector = new Vector3(); 
        switch (direction)
        {
            case Direction.NORTH:
                return vector.North();
            case Direction.SOUTH:
                return vector.South();
            case Direction.EAST:
                return vector.East();
            case Direction.WEST:
                return vector.West();
            case Direction.NORTH_WEST:
                return vector.NorthWest();
            case Direction.SOUTH_WEST:
                return vector.SouthWest();
            case Direction.NORTH_EAST:
                return vector.NorthEast();
            case Direction.SOUTH_EAST:
                return vector.SouthEast();
            default:
                return Vector3.zero;
        }
    }
}