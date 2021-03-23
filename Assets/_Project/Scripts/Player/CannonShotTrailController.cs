using System;
using UnityEngine;

public class CannonShotTrailController : MonoBehaviour
{
    [SerializeField] private CannonShot cannonShot;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject sparkleTrail;
    
    private bool _trailActive;

    private void Awake()
    {
        TurnOffTrail();
        cannonShot.OnDie += TurnOffTrail;
        _trailActive = false;
    }

    private void Update()
    {
        if (cannonShot.HasFired && _trailActive == false)
            TurnOnTrail();
    }

    private void TurnOffTrail()
    {
        _trailActive = false; 
        trail.SetActive(false);
        sparkleTrail.SetActive(false);
    }

    private void TurnOnTrail()
    {
        _trailActive = true;
        trail.SetActive(true);
        sparkleTrail.SetActive(true);
    }
}
