using UnityEngine;

[CreateAssetMenu(menuName = "SwayProfile")]
public class SwayProfile : ScriptableObject
{
    [SerializeField] public AnimationCurve profile;
    [SerializeField] public float lowRangeTime;
    [SerializeField] public float highRangeTime;
    [SerializeField] public float distanceFromCenter;
}