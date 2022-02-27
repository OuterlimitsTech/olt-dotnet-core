using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class SystemIOTests
    {

        private const string EmbeddedFile = "ImportTest.xlsx";

        [Fact]
        public void NameWithoutExtTest()
        {
            var fileInfo = new FileInfo(EmbeddedFile);
            Assert.Equal("ImportTest", OltSystemIOExtensions.NameWithoutExt(fileInfo));
        }

        [Fact]
        public void ToFile()
        {
            var dir = UnitTestHelper.BuildTempPath();

            try
            {
                var fileName = Path.Combine(dir, $"{nameof(ToFile)}_{Guid.NewGuid()}.xlsx");
                using (var resource = this.GetType().Assembly.GetEmbeddedResourceStream(EmbeddedFile))
                {
                    try
                    {
                        var bytes = OltSystemIOExtensions.ToBytes(resource);                        

                        using (var ms = OltSystemIOExtensions.ToMemoryStream(bytes))
                        {
                            OltSystemIOExtensions.ToFile(ms, fileName);
                            Assert.True(File.Exists(fileName));
                        }

                        using (var ms = OltSystemIOExtensions.ToMemoryStream(bytes))
                        {
                            OltSystemIOExtensions.ToFile(ms, fileName); //override the file
                            Assert.True(File.Exists(fileName));
                        }
                                                
                    }
                    catch(Exception ex)
                    {
                        Assert.True(false, ex.ToString());
                    }
                }
 
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void FileToBytes()
        {
            var dir = UnitTestHelper.BuildTempPath();
            try
            {
                var fileName = Path.Combine(dir, $"{nameof(FileToBytes)}_{Guid.NewGuid()}.xlsx");

                using (var resource = this.GetType().Assembly.GetEmbeddedResourceStream(EmbeddedFile))
                {
                    try
                    {
                        var bytes = OltSystemIOExtensions.ToBytes(resource);
                        Assert.True(true);

                        using (var ms = OltSystemIOExtensions.ToMemoryStream(bytes))
                        {
                            OltSystemIOExtensions.ToFile(ms, fileName);
                        }

                        var expectedBytes = File.ReadAllBytes(fileName);
                        Assert.True(File.Exists(fileName));
                        Assert.Equal(expectedBytes.Length, bytes.Length);
                    }
                    catch (Exception ex)
                    {
                        Assert.True(false, ex.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void FileToMemoryStream()
        {
            var dir = UnitTestHelper.BuildTempPath();
            try
            {
                var fileName = Path.Combine(dir, "FileToMemoryStream_Import.xlsx");
                var ms = this.GetType().Assembly
                    .GetEmbeddedResourceStream(EmbeddedFile)
                    .ToBytes()
                    .ToMemoryStream();
                ms.ToFile(fileName);
                var msCopy = OltSystemIOExtensions.ToMemoryStream(File.ReadAllBytes(fileName));
                Assert.Equal(ms.Length, msCopy.Length);
                Assert.True(File.Exists(fileName));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void BytesToFile()
        {
            var dir = UnitTestHelper.BuildTempPath();
            try
            {
                var fileName = Path.Combine(dir, "BytesToFile_Import.xlsx");

                var bytes = this.GetType().Assembly
                    .GetEmbeddedResourceStream(EmbeddedFile)
                    .ToBytes();

                bytes.ToFile(fileName);
                var copyBytes = File.ReadAllBytes(fileName);

                Assert.True(File.Exists(fileName));
                Assert.Equal(bytes.Length, copyBytes.Length);

                copyBytes.ToFile(fileName); //Checking File Exists
                Assert.True(File.Exists(fileName));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void FileInfoToBytes()
        {
            var dir = UnitTestHelper.BuildTempPath();
            try
            {
                var fileName = Path.Combine(dir, "BytesToFile_Import.xlsx");

                this.GetType().Assembly
                    .GetEmbeddedResourceStream(EmbeddedFile)
                    .ToBytes()
                    .ToFile(fileName);

                var fileInfo = new FileInfo(fileName);
                var bytes = fileInfo.ToBytes();

                var copyBytes = File.ReadAllBytes(fileName);

                Assert.Equal(bytes.Length, copyBytes.Length);
                Assert.True(File.Exists(fileName));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }



    }
}
