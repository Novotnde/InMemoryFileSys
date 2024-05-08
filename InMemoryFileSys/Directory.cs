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
    
    private static readonly Lazy<Directory> _root = 
        new(() => new Directory(slash, new SystemClock(), null));

    private readonly IClock _clock;

    private readonly Directory? _parentDirectory;
    
    private const string slash = "/";
    
    private Directory(string? name, IClock clock, Directory? parentDirectory)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));

        Name = name;
        CreationDate = _clock.UtcNow;
        
        _parentDirectory = parentDirectory;

        if (parentDirectory == null)
        {
            Path = slash;
        }
        else
        {
            Path = parentDirectory.Path.EndsWith(slash) ? parentDirectory.Path + name : parentDirectory.Path + slash + name;
        }

        _entries = new Dictionary<string, IFileSystemEntry>();
    }

    /// <inheritdoc/>
    public IFile CreateFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Contains(slash))
            throw new ArgumentException("File name cannot be null, empty, or contain directory separators.");

        if (_entries.ContainsKey(fileName))
        {
            throw new ArgumentException("A file with the same name already exists in this directory.");
        }

        var fullPath = Path.EndsWith(slash) ? Path + fileName : Path + slash + fileName;

        var file = new File(fullPath)
        {
            Name = fileName,
            CreationDate = _clock.UtcNow,
        };

        _entries[fileName] = file;
        UpdateSize(file.Size ?? 0);  
        return file; 
    }

    /// <inheritdoc/>

    public IDirectory CreateDirectory(string directoryName)
    {
        if (string.IsNullOrWhiteSpace(directoryName) || directoryName.Contains(slash))
            throw new ArgumentException("Directory name cannot be null, empty, or contain directory separators.");

        if (_entries.ContainsKey(directoryName))
        {
            throw new ArgumentException("A directory with the same name already exists in this directory.");
        }

        var newDirectory = new Directory(directoryName, _clock, this)
        {
            Name = directoryName
        };

        _entries[directoryName] = newDirectory;

        return newDirectory; 
    }


    private void UpdateSize(int fileSize)
    {
        Size += fileSize;
        _parentDirectory?.UpdateSize(fileSize);
    }
}