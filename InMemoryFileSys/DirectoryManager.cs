namespace InMemoryFileSys;

public static class DirectoryManager
{
    private static readonly IDirectory RootDirectory = new Directory ("/");
    
    public static IDirectory GetRootDirectory()
    {
        return RootDirectory;
    }
}