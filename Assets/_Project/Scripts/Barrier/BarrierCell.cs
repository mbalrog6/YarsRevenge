using UnityEngine;

public class BarrierCell : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Dimensions cellDimensions;

    public int Score { get; private set; } = 5; 
    public float Width => cellDimensions.x / 4f;
    public float Height => cellDimensions.y / 4f;
}