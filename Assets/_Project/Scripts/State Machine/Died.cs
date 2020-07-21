public class Died : IState
{
    private Warlord warlord;

    public Died( Warlord warlord)
    {
        this.warlord = warlord;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        warlord.gameObject.SetActive(false);
    }

    public void OnExit()
    {
        
    }
}