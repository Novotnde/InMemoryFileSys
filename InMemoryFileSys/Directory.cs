using InMemoryFileSys.Contracts;

namespace InMemoryFileSys;

/// <inheritdoc/>
public class Directory : IDirectory
{
    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    // Public property to access the singleton instance
    public static Directory Root => _root.Value;   
    
    /// <inheritdoc/>
    public int? Size { get; private set; }
   
    public string Path { get; }

    private readonly Dictionary<string, IFileSystemEntry> _entries;
    
    // Singleton instance
    // Singleton instance
    private static readonly Lazy<Directory> _root = 
        new(() => new Directory(rootName, new SystemClock(), null));    
    private readonly IClock _clock;

    private readonly Directory? _parentDirectory;
    
    private const string slash = "/";
    private const string rootName = "/";
    
    private Directory(string? name, IClock clock, Directory? parentDirectory)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));

        Name = name;
        CreationDate = _clock.UtcNow;
        
        _parentDirectory = parentDirectory;

        if (parentDirectory != null)
        {
            if (parentDirectory.Path == slash)
            {
                Path = parentDirectory.Path + name;
            }
            else
            {
                Path = parentDirectory.Path + slash + name;
            }
        }
        else
        {
            Path =  name;
        }
        
        _entries = new Dictionary<string, IFileSystemEntry>();
    }

    /// <inheritdoc/>

    /// <inheritdoc/>
    public IFile AddFile(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("File name cannot be null or whitespace.");

        if (relativePath.StartsWith(slash))
        {
            relativePath = relativePath.Substring(1);
        }

        if (_entries.ContainsKey(relativePath))
        {
            throw new ArgumentException("File already exists at the specified path.");
        }

        var parts = relativePath.Split(slash);

        var directoryPath = string.Join(slash, parts.Take(parts.Length - 1));
        var fileName = parts.Last();

        var directory = GetDirectoryOrCreate(directoryPath);
        var file = new File(directory.Path + relativePath)
        {
            Name = fileName,
            CreationDate = _clock.UtcNow,
        };
        directory._entries[fileName] = file;
        directory.UpdateSize(file.Size ?? 0);

        return file; // Return the newly added file.
    }

    /// <inheritdoc/>

    public IDirectory AddDirectory(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("Directory path cannot be null or whitespace.");

        var parts = relativePath.Split(slash);
        var directoryPath = string.Join(slash, parts.Take(parts.Length - 1));
        var directoryName = parts[^1];

        var parentDirectory = GetDirectoryOrCreate(directoryPath);
        var newDirectory = new Directory(directoryName, _clock, parentDirectory);
        parentDirectory._entries[directoryName] = newDirectory;
    
        newDirectory.Name = directoryName;

        return newDirectory;
    }
    
    private Directory GetDirectoryOrCreate(string path)
    {
        var currentDir = this;
        foreach (var part in path.Split(slash))
        {
            if (string.IsNullOrEmpty(part))
                continue;

            if (!currentDir._entries.TryGetValue(part, out var subDir))
            {
                subDir = new Directory(part, _clock, currentDir);
                currentDir._entries[part] = subDir;
            }
            currentDir = subDir as Directory ?? throw new InvalidOperationException("Expected a directory.");
        }
        return currentDir;
    }

    private void UpdateSize(int fileSize)
    {
        Size += fileSize;
        _parentDirectory?.UpdateSize(fileSize);
    }
}