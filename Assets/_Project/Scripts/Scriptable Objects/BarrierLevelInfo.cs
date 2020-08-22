using UnityEngine;

[CreateAssetMenu(menuName = "Level/Barrier Level Info")]
public class BarrierLevelInfo : ScriptableObject
{
    [SerializeField] public int barrierTypeIndex;
    [SerializeField] public float osalationSpeed;
    [SerializeField] public float barrierRotationSpeed;
    [SerializeField] public bool barrierReflectsCannon;
}