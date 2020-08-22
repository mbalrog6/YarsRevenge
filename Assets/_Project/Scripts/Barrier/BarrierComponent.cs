using UnityEngine;

public class BarrierComponent : MonoBehaviour
{
    public int Height;
    public int Width;
    public float CellWidth;
    public float CellHeight;
    public BarrierCell[] Barrier;
    public BarrierInfo BarrierInfo { get; private set; }

    public void SetBarrierInfo(BarrierInfo barrierInfo)
    {
        this.BarrierInfo = barrierInfo;
    }
}
