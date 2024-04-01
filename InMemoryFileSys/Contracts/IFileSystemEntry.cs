namespace InMemoryFileSys;

/// <summary>
/// Represents an entry in the file system.
/// </summary>
public interface IFileSystemEntry
{
    /// <summary>
    /// The name of the file system entry.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// The creation date of the file system entry.
    /// </summary>
    public DateTime CreationDate { get; set; }
    
    /// <summary>
    /// The size of the file system entry.
    /// </summary>
    public int? Size { get; }
}