namespace InMemoryFileSys;

/// <summary>
/// Represents a file in the file system.
/// </summary>
public interface IFile : IFileSystemEntry
{
    /// <summary>
    /// The content of the file.
    /// </summary>
    public byte[] Content { get; set; }
}
