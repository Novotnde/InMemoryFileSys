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
            root.CreateFile("file1.txt");
            root.CreateFile("file2.txt");

            var subdir1 = root.CreateDirectory("subdir1");
            subdir1.CreateFile("file3.txt");

            var subdir2 = root.CreateDirectory("subdir2");
            var subsubdir = subdir2.CreateDirectory("subsubdir");
            subsubdir.CreateFile("file4.txt");

            // Handle exceptions and try to create duplicates
            try
            {
                root.CreateFile("file1.txt");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Failed to add duplicate file: {e.Message}");
            }

            try
            {
                root.CreateFile("");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Failed to add file: {e.Message}");
            }

            Console.WriteLine($"Root directory size: {root.Size} bytes");
            Console.ReadLine();

        }
    }
}