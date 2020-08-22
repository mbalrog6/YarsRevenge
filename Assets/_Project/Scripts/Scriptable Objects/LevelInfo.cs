using UnityEngine;

[CreateAssetMenu(menuName = "Level/Level Info")]
public class LevelInfo : ScriptableObject
{
    [Header("Qotile")] 
    [SerializeField] public QotileInfo QotileInfo;

    [Header("Probe")] 
    [SerializeField] public ProbeInfo ProbeInfo;

    [Header("Barrier")]
    [SerializeField] public BarrierInfo BarrierInfo;

    public bool HasIonField;
}