public interface IBarrier
{
    BarrierCell[] GetCellArray { get; }
    bool IsCellActive(int index);
}
