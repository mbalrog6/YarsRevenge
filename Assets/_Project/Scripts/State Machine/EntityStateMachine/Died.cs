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
    }

    public void OnExit()
    {
    }
}