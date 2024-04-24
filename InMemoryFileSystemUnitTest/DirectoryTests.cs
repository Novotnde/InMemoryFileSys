using NUnit.Framework;
using System;

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
            var relativePath = "test.txt";

            var file = _rootDirectory.CreateFile(relativePath);

            var path = file.Path;
            Assert.IsNotNull(path);
            Assert.IsTrue(path.EndsWith(relativePath));
        }

        [Test]
        public void AddFile_NullOrWhiteSpaceRelativePath_ThrowsArgumentException()
        {
            string relativePath = null;

            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateFile(relativePath));
        }

        [Test]
        public void AddDirectory_ValidRelativePath_DirectoryAddedSuccessfully()
        {

            var relativePath = "newDirectory";

            var newDirectory = _rootDirectory.CreateDirectory(relativePath);

            var directoryPath = newDirectory.Path;
            Assert.IsNotNull(directoryPath);
            Assert.IsTrue(directoryPath.EndsWith(relativePath));
            Assert.IsInstanceOf<Directory>(newDirectory);
        }

        [Test]
        public void AddDirectory_NullOrWhiteSpaceRelativePath_ThrowsArgumentException()
        {
            string relativePath = null;

            Assert.Throws<ArgumentException>(() => _rootDirectory.CreateDirectory(relativePath));
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
            Assert.AreEqual("/folder1/folder2", path);
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
