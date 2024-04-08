using InMemoryFileSys.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InMemoryFileSys
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IClock, SystemClock>()
                .BuildServiceProvider();

            var root = Directory.Root;
            // Add files and directories
            root.AddFile("file1.txt");
            root.AddFile("file2.txt");
            root.AddDirectory("subdir1");
            root.AddFile("subdir1/file3.txt");
            root.AddDirectory("subdir2");
            root.AddDirectory("subdir2/subsubdir");
            root.AddFile("subdir2/subsubdir/file4.txt");

            var subDirectory = root.AddDirectory("subdir1");
            var fileName = "file.txt";
            subDirectory.AddFile(fileName);
            Console.WriteLine(subDirectory.Path);
            // Add duplicate file
            try
            {
                root.AddFile("file1.txt");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Failed to add duplicate file: {e.Message}");
            }

            // Access non-existent file
            try
            {
                root.AddFile("");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Failed to add file: {e.Message}");
            }
            
            // Print root directory size
            Console.WriteLine($"Root directory size: {root.Size} bytes");

            Console.ReadLine(); 
        }
    }
}