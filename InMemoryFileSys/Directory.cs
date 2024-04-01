namespace InMemoryFileSys;

/// <inheritdoc/>
public class Directory : IDirectory
{
    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    /// <inheritdoc/>
    public int? Size
    {
        get
        {
            int totalSize = 0;
            foreach (var entry in _entries)
            {
                if (entry is IFile file)
                {
                    totalSize += file.Size ?? 0;
                }
                else if (entry is IDirectory directory)
                {
                    totalSize += directory.Size ?? 0;
                }
            }

            return totalSize;
        }
    }
    
    private List<IFileSystemEntry> _entries { get; }
    
    internal Directory(string? name)
    {
        Name = name;
        _entries = new List<IFileSystemEntry>();
    }
    
    /// <inheritdoc/>
    public void AddEntry(string name, bool isFile)
    {
        if (ContainsEntry(name))
            return;

        string?[] parts = name.Split('/');
        if (parts.Length == 1 && isFile)
        {
            _entries.Add(CreateFile(name));
            return;
        }

        var currentDir = this;
        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part))
                continue;

            var subDir = currentDir.GetSubDirectory(part);
            if (subDir == null)
            {
                if (!isFile)
                {
                    currentDir._entries.Add(CreateDirectory(part));
                }
                else
                {
                    currentDir._entries.Add(CreateFile(part));
                }
            }
            currentDir = subDir;
        }
    }

    /// <inheritdoc/>
    public string? GetPath(string name)
    {
        if (name == "/") return DirectoryManager.GetRootDirectory().Name;

        var parts = name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        return FindEntryPathRecursive(this, parts, 0);
    }
    
    private IFileSystemEntry CreateFile(string? fileName)
    {
        return new File
        {
            Name = fileName,
            CreationDate = DateTime.UtcNow
        };
    }

    private IFileSystemEntry CreateDirectory(string? dirName)
    {
        return new Directory(dirName)
        {
            CreationDate = DateTime.UtcNow
        };
    }

    private bool ContainsEntry(string name)
    {
        var value = _entries.Any(x => x.Name == name);
        return value;
    }


    private Directory? GetSubDirectory(string? name)
    {
        foreach (var entry in _entries)
        {
            if (entry is Directory directory && directory.Name == name)
            {
                return directory;
            }
        }

        return null;
    }

    private string? FindEntryPathRecursive(Directory directory, string[] parts, int index)
    {
        if (index >= parts.Length) return null;

        var currentPart = parts[index];

        var item = directory._entries.FirstOrDefault(x => x.Name == currentPart);

        if (index == parts.Length - 1) return item != null ? "/" + item.Name : "No file found";

        if (item is Directory subdirectory)
        {
            var subdirectoryPath = FindEntryPathRecursive(subdirectory, parts, index + 1);
            return subdirectoryPath != null ? "/" + item.Name + subdirectoryPath : null;
        }

        return null;
    }

}