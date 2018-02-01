using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRunner.SampleTests
{
    [TestClass]
    public class SampleTests
    {
        [TestMethod]
        public void PassingTest()
        {

        }

        [TestMethod]
        public void FailingTest()
        {
            throw new Exception("FailingTest");
        }
    }
}
