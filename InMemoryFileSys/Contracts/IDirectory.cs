namespace InMemoryFileSys.Contracts;

/// <summary>
/// Represents a directory in the file system.
/// </summary>
public interface IDirectory : IFileSystemEntry
{
    /// <summary>
    /// Adds a new file.
    /// </summary>
    /// <param name="relativePath"></param>
    public IFile AddFile(string relativePath);
    
    /// <summary>
    /// Adds a new  directory.
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public IDirectory AddDirectory(string relativePath);
}