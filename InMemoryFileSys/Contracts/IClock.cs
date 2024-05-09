namespace InMemoryFileSys.Contracts;

public interface IClock
{
    DateTime UtcNow { get; }
}