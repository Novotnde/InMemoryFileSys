namespace InMemoryFileSys;

public interface IClock
{
    DateTime UtcNow { get; }
}