using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JavaPort;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass()]
    public class TestInitialize
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Extensions.UnitTestState.IsInUnitTest = true;

            Assembly assembly = typeof(TestInitialize).GetTypeInfo().Assembly;
            foreach (string resourceFileName in assembly.GetManifestResourceNames().ToList())
            {
                using (Stream resourceStream = assembly.GetManifestResourceStream(resourceFileName))
                {
                    byte[] buffer = new byte[resourceStream.Length];
                    resourceStream.Read(buffer, 0, (int)resourceStream.Length);

                    Extensions.UnitTestState.TestResourceStreams.Add(resourceFileName, buffer);
                }
            }
        }
    }
}
