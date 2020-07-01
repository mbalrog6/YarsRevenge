public interface IPlayerInput
{
    float Horizontal { get; }
    float Vertical { get; }

    Direction PlayerDirection { get; }
    Direction LastPlayerDirection { get; }
    
    Direction LastPlayerFacing { get; }

    void Tick();
}
