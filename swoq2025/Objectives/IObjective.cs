namespace swoq2025.Objectives;

public interface IObjective
{
    public int Priority { get; }

    public bool IsCompleted { get; }

    public bool CanBeSolved { get; }

    public bool HasToBeSolved { get; }

    public bool TryGetNextTarget(out Coord target);

    public void Reset();
}
