namespace InMemoryFileSys.Tests
{
    [TestFixture]
    public class DirectoryTests
    {
        private Directory _rootDirectory;

        [SetUp]
        public void Setup()
        {
            _rootDirectory = Directory.Root;
        }

        [Test]
        public void AddFile_ValidRelativePath_FileAddedSuccessfully()
        {
            var file = _rootDirectory.CreateFile("test.txt");

            var path = file.Path;
            Assert.IsNotNull(path);
            Assert.That(path, Is.EqualTo("/test.txt"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CreateFile_InvalidName_ThrowsArgumentException(string fileName)
        {
            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateFile(fileName));
        }

        [Test]
        public void AddDirectory_ValidRelativePath_DirectoryAddedSuccessfully()
        {

            var directory = _rootDirectory.CreateDirectory("newDirectory");

            Assert.AreEqual("/newDirectory", directory.Path);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CreateDirectory_InvalidName_ThrowsArgumentException(string directoryName)
        {
            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateDirectory(directoryName));
        }

        [Test]
        public void Path_RootDirectory_ReturnsRootName()
        {
            var path = _rootDirectory.Path;

            Assert.AreEqual("/", path);
        }

        [Test]
        public void Path_NestedDirectory_ReturnsCorrectPath()
        {
            var folder1 = _rootDirectory.CreateDirectory("folder1");
            var folder2 = folder1.CreateDirectory("folder2");
            folder2.CreateFile("file.txt");

            var path = folder2.Path;

            Assert.IsNotNull(path);
            Assert.That(path, Is.EqualTo("/folder1/folder2"));
        }

        [Test]
        public void AddDuplicateFile_ThrowsArgumentException()
        {
            _rootDirectory.CreateFile("file1.txt");

            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateFile("file1.txt"));
        }

        [Test]
        public void AddNonExistentFile_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateFile(""));
        }

        [Test]
        public void AddNonExistentDirectory_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateDirectory(""));
        }
    }
}
