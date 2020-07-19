using UnityEngine;

public class BarrierCell : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Dimensions cellDimensions;

    public int Width => cellDimensions.x;
    public int Height => cellDimensions.y;
    
}