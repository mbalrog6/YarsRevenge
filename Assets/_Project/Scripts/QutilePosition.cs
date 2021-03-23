using UnityEngine;

public class QutilePosition : MonoBehaviour
{
    private Vector3 _position;
    private Barrier2 _barrier;

    void Update()
    {
        if (_barrier == null)
        {
            _barrier = FindObjectOfType<Barrier2>();
            return;
        }
        
        _position = _barrier.WarlordSpawnPoint;
        transform.position = _position;
    }
    
    
}
