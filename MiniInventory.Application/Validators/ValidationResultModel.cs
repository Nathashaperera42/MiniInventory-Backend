namespace MiniInventory.Application.Validators;

public class ValidationResultModel
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = new();

    public void AddIf(bool condition, string error)
    {
        if (condition) Errors.Add(error);
    }
}
