using System.Collections.Generic;
using UnityEngine;

public enum EntityCast
{
    Player,
    Warlord,
    Probe, 
    Cannon, 
    Barrier,
}

public class PlayerCollisions
{
    private Dictionary<EntityCast, RectContainer> _entityRects;

    public PlayerCollisions( RectContainer playerRect )
    {
        _entityRects = new Dictionary<EntityCast, RectContainer>();
        _entityRects[EntityCast.Player] =  playerRect;
    }

    public void AddEntityRect(EntityCast entityCast, RectContainer entityRect)
    {
        _entityRects[entityCast] = entityRect;
    }

    public bool CheckForRectCollision(Rect originator, Rect targetObject)
    {
        return originator.Overlaps(targetObject);
    }
    
    public bool CheckIfPlayerHit(RectContainer entity)
    {
        return _entityRects[EntityCast.Player].Bounds.Overlaps(entity.Bounds);
    }
    
    public bool CheckIfPlayerHit(EntityCast entity)
    {
        return _entityRects[EntityCast.Player].Bounds.Overlaps(_entityRects[entity].Bounds);
    }

    public bool CheckPlayerRadiusWithinEntityRadius(EntityCast entity, float playerRadius, float entityRadius)
    {
        var dist =  _entityRects[entity].Bounds.center - _entityRects[EntityCast.Player].Bounds.center;
        return dist.sqrMagnitude < (playerRadius + entityRadius) * (playerRadius + entityRadius);
    }
}
