using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Qotile Info")]
[Serializable]
public class QotileInfo : ScriptableObject
{
    [SerializeField] public float chargeUpTimeMin;
    [SerializeField] public float chargeUpTimeMax;
    [SerializeField] public int chargeUpAdvancementRate;
    [SerializeField] public float chargeUpDecrementValue;
    [SerializeField] public float launchSpeedMin;
    [SerializeField] public float launchSpeedMax;
    [SerializeField] public int launchSpeedAdvancementRate;
    [SerializeField] public float launchSpeedIncrementValue;
}