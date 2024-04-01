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

    /// <inheritdoc/>
    public byte[] Content { get; set; }


}