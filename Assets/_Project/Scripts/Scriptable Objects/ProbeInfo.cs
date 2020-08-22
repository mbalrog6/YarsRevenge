using UnityEngine;

[CreateAssetMenu(menuName = "Level/Probe Info")]
public class ProbeInfo : ScriptableObject
{
    [SerializeField] public float probeSpeedMin;
    [SerializeField] public float probeSpeedMax;
    [SerializeField] public float speedIncrementRate;
    [SerializeField] public float speedIncrementTimer;
    [SerializeField] public float probeRespawnTimerMin;
    [SerializeField] public float probeRespawnTimerMax;
    [SerializeField] public float probeRespawnRateChange;
    [SerializeField] public float probeRespawnChangeTimer;
}