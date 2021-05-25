using UnityEngine;

public class CenterExplosionTransitionObject : MonoBehaviour
{
    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<ExplosionTransionStartCommand>(CenterObjectOnLocation);
    }

    public void CenterObjectOnLocation(ExplosionTransionStartCommand explosionTransionStartCommand)
    {
        _myTransform.position = new Vector3(explosionTransionStartCommand.Position.x, explosionTransionStartCommand.Position.y, _myTransform.position.z);
        Debug.Log($"x: {explosionTransionStartCommand.Position.x}, y: {explosionTransionStartCommand.Position.y} - my pos x: {_myTransform.position.x}, y: {_myTransform.position.y}");
    }
}
