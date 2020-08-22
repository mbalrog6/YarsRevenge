using UnityEngine;

[CreateAssetMenu(menuName = "Level/Barrier Info")]
public class BarrierInfo : ScriptableObject
{
    public int BarrierIndex;
    public bool IsReflective;
    public BarrierShiftPatterns ShiftPattern;
    public float BarrierShiftPulseTime;
}