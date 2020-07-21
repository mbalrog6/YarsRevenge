using UnityEngine;

public class BarrierCell : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Dimensions cellDimensions;

    public int Score { get; private set; } = 5; 
    public int Width => cellDimensions.x;
    public int Height => cellDimensions.y;
}