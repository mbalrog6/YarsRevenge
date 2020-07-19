using UnityEngine;

public class OsalateMover : IMover
{
    public float Speed { get; set; }
    public bool OscalatingInY { get; set; }
    public bool MovingTowardHighBound { get; private set; }
    
    private GameObject _entity;
    private Transform _entityTransform;

    private float[] oscalationBounds;
    private Vector3 _computedPosition;


    public OsalateMover(GameObject entity, float highBound, float lowBound, float speed)
    {
        _entity = entity;
        _entityTransform = entity.transform;
        oscalationBounds = new float[2];
        Speed = speed;
        OscalatingInY = true; 
        SetOscalationBounds(lowBound, highBound);
    }

    private void SetOscalationBounds(float lowBound, float highBound)
    {
        oscalationBounds[0] = lowBound;
        oscalationBounds[1] = highBound;
    }

    public void Tick()
    {
        _computedPosition = _entityTransform.position;
        float delta = Time.deltaTime * Speed;
        if (OscalatingInY)
        {
            _computedPosition.y += (MovingTowardHighBound ? delta : -delta);
            _computedPosition.y = Mathf.Min(oscalationBounds[1], _computedPosition.y);
            _computedPosition.y = Mathf.Max(oscalationBounds[0], _computedPosition.y);
            if (_computedPosition.y == oscalationBounds[0])
            {
                MovingTowardHighBound = true; 
            } else if (_computedPosition.y == oscalationBounds[1])
            {
                MovingTowardHighBound = false; 
            }
        }
        else
        {
            _computedPosition.x += (MovingTowardHighBound ? delta : -delta);
            _computedPosition.x = Mathf.Min(oscalationBounds[1], _computedPosition.x);
            _computedPosition.x = Mathf.Max(oscalationBounds[0], _computedPosition.x);
            if (_computedPosition.x == oscalationBounds[0])
            {
                MovingTowardHighBound = true; 
            } else if (_computedPosition.x == oscalationBounds[1])
            {
                MovingTowardHighBound = false; 
            }
        }

        _entityTransform.position = _computedPosition;
    }
    
}
