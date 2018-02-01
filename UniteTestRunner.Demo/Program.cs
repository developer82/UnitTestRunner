using System;
using UnitTestRunner;
using UnitTestRunner.SampleTests;

namespace UniteTestRunner.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRunner testRunner = new TestRunner();

            SampleTests tests = new SampleTests();
            testRunner.RunTest(tests.PassingTest, "This test should pass", "OK", "FAILED");
            testRunner.RunTest(tests.FailingTest, "This test should fail", "OK", "FAILED");

            testRunner.RunTestClass(typeof(SampleTests));

            Console.ReadLine();
        }
    }
}
