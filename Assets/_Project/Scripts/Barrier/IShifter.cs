public interface IShifter
{
    void Tick();
    BarrierShiftPatterns Pattern { get; set; }
}