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

        if (parentDirectory == null)
        {
            Path = name;
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
        // Check if the file name is null, empty, or contains slashes (which are not allowed)
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Contains(slash))
            throw new ArgumentException("File name cannot be null, empty, or contain directory separators.");

        // Check if the file already exists in this directory
        if (_entries.ContainsKey(fileName))
        {
            throw new ArgumentException("A file with the same name already exists in this directory.");
        }

        // Create the new file
        var file = new File(Path + slash + fileName)
        {
            Name = fileName,
            CreationDate = _clock.UtcNow,
            // Assuming a default size or some means to set the size here
        };

        // Add the file to the current directory's entries
        _entries[fileName] = file;
        UpdateSize(file.Size ?? 0);  // Update the size of the directory, if applicable

        return file; // Return the newly added file.
    }

    /// <inheritdoc/>

    public IDirectory CreateDirectory(string directoryName)
    {
        // Check if the directory name is null, empty, or contains slashes (which are not allowed)
        if (string.IsNullOrWhiteSpace(directoryName) || directoryName.Contains(slash))
            throw new ArgumentException("Directory name cannot be null, empty, or contain directory separators.");

        // Check if a directory with the same name already exists
        if (_entries.ContainsKey(directoryName))
        {
            throw new ArgumentException("A directory with the same name already exists in this directory.");
        }

        // Create the new directory as a child of the current directory
        var newDirectory = new Directory(directoryName, _clock, this)
        {
            Name = directoryName
        };

        // Add the new directory to the current directory's entries
        _entries[directoryName] = newDirectory;

        return newDirectory; // Return the newly added directory.
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