namespace InMemoryFileSys;

/// <inheritdoc/>
public class File : IFile
{
    /// <inheritdoc/>
    public string? Name { get; set; }
    
    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    /// <inheritdoc/>
    public int? Size { get; }

    public string Path { get; }

    /// <inheritdoc/>
    public byte[] Content { get; set; }

    public File(string relativePath)
    {
        Path = relativePath;
    }
}