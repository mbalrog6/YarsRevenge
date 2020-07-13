public interface IPlayerInput
{
    InputDTO Inputs { get; }
    void Tick();

    void CopyDTO(ref InputDTO inputDTO);
    void SetInput(InputDTO inputs);
}
