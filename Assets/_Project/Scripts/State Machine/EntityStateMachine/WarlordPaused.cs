using UnityEngine;

public class WarlordPaused : IState
{
    private EntityStateMachine _stateMachine;
    private Warlord _warlord;
    private Vector3 _offScreen;
    
    public WarlordPaused(EntityStateMachine stateMachine, Warlord warlord)
    {
        _stateMachine = stateMachine;
        _warlord = warlord;
        _offScreen = new Vector3(200f, 200f, _warlord.transform.position.z);
    }

    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        _warlord.transform.position = _offScreen;
    }

    public void OnExit()
    {
        
    }
}
