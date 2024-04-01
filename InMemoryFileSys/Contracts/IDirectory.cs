namespace InMemoryFileSys;

/// <summary>
/// Represents a directory in the file system.
/// </summary>
public interface IDirectory : IFileSystemEntry
{
    /// <summary>
    /// Adds a new entry (file or directory) to the directory.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <param name="isFile">True if the entry being added is a file; otherwise, false for a directory.</param>
    public void AddEntry(string name, bool isFile);
    
    /// <summary>
    /// Gets the full path of an entry within the directory.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The full path of the entry if found; otherwise, null.</returns>
    public string? GetPath(string name);
}