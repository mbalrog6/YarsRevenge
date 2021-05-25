public class AmmoUpdateCommand : ICommand
{
    public AmmoUpdateCommand(int ammoValue)
    {
        AmmoValue = ammoValue;
    }

    public int AmmoValue { get; set; }
}
