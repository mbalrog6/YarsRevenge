using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    public Vector3 DirectionVector => CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInput.Inputs.Direction);
    public Vector3 LastFacingDirectionVector => CardinalDirections.GetUnitVectorFromCardinalDirection(PlayerInput.Inputs.LastFacingDirection);
    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    private IMover _mover;
    private IRotator _rotator;
    public IPlayerInput PlayerInput { get; set; }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        _mover = new KineticMover(this);
        _rotator = new DirectionalRotator(this);
    }

    private void Update()
    {
        PlayerInput.Tick();
        _rotator.Tick();
        _mover.Tick();
    }
}