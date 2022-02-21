using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Extensions.General.Tests
{
    public static class UnitTestHelper
    {
        public const string FileNamePrefix = "OLT_Extensions_General_Tests_";

        public static string BuildTempPath(string rootDir)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), rootDir, $"{FileNamePrefix}{Guid.NewGuid()}");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            return tempDir;
        }

        public static string BuildTempPath()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), $"{FileNamePrefix}{Guid.NewGuid()}");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            return tempDir;
        }

    }
}
