using Moq;
using InMemoryFileSys;

namespace InMemoryFileSystemUnitTest
{
    public class DirectoryTests
    {
        private IDirectory _rootDirectory;
        private Mock<IClock> _clockMock;

        [SetUp]
        public void Setup()
        {
            var mockClock = new Mock<IClock>();
            mockClock.Setup(clock => clock.UtcNow).Returns(new DateTime(2024, 4, 1));

            _rootDirectory = InMemoryFileSys.Directory.Root;
        }
        
        [Test]
        public void AddEntry_AddsFileToDirectory()
        {
            var expectedFileName = "file1.txt";

            _rootDirectory.AddEntry(expectedFileName, isFile: true);

            var path = _rootDirectory.GetPath(expectedFileName);
            Assert.AreEqual("/" + expectedFileName, path);
        }

        [Test]
        public void GetPath_ReturnsCorrectPathForFile()
        {
            _rootDirectory.AddEntry("subdir", isFile: false);
            _rootDirectory.AddEntry("subdir/file.txt", isFile: true);

            var path = _rootDirectory.GetPath("subdir/file.txt");

            Assert.AreEqual("/subdir/file.txt", path);
        }

    }
}