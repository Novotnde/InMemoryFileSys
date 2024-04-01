
namespace InMemoryFileSys
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var root = DirectoryManager.GetRootDirectory();

            // Add files and directories
            root.AddEntry("file1.txt", true);
            root.AddEntry("file2.txt", true);
            root.AddEntry("subdir1", false);
            root.AddEntry("subdir1/file3.txt", true);
            root.AddEntry("subdir2", false);
            root.AddEntry("subdir2/subsubdir", false);
            root.AddEntry("subdir2/subsubdir/file4.txt", true);

            // Test GetPath method with various scenarios
            Console.WriteLine(root.GetPath("file1.txt")); // Should output: /file1.txt
            Console.WriteLine(root.GetPath("file3.txt")); // Should output: No file found
            Console.WriteLine(root.GetPath("subdir1/file3.txt")); // Should output: /subdir1/file3.txt
            Console.WriteLine(root.GetPath("subdir2/subsubdir/file4.txt")); // Should output: /subdir2/subsubdir/file4.txt

            // Add duplicate file
            root.AddEntry("file1.txt", true);

            // Access non-existent file
            Console.WriteLine(root.GetPath("file5.txt")); 

            // Print root directory size
            Console.WriteLine($"Root directory size: {root.Size} bytes");

            Console.ReadLine(); 
        }
    }
}