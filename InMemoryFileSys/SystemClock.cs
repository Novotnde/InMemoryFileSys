namespace InMemoryFileSys;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}