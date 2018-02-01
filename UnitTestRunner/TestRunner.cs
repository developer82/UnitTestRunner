using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentColorConsole;

namespace UnitTestRunner
{
    public class TestRunner
    {
        public bool RunTest(Action testMethod)
        {
            try
            {
                testMethod.Invoke();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool RunTest(Action testMethod, string testText, string passText, string failText)
        {
            return RunTest(testMethod, testText, passText, failText, false);
        }

        public bool RunTest(Action testMethod, string testText, string passText, string failText, bool prepend)
        {
            if (!prepend)
                Console.Write($"{testText}... ");

            bool testPassed = RunTest(testMethod);
            if (testPassed)
            {
                if (prepend)
                    ColorConsole.WithGreenText.Write($"[{passText}]");
                else
                    ColorConsole.WithGreenText.WriteLine($"[{passText}]");
            }
            else
            {
                if (prepend)
                    ColorConsole.WithRedText.Write($"[{failText}]");
                else
                    ColorConsole.WithRedText.WriteLine($"[{failText}]");
            }

            if (prepend)
                Console.WriteLine($" {testText}");

            return testPassed;
        }

        public void RunTestClass(Type testClass)
        {
            RunTestClass(testClass, false);
        }

        public void RunTestClass(Type testClass, bool prepend)
        {
            MethodInfo[] methodInfos = testClass.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            object testClassInstance = Activator.CreateInstance(testClass);
            foreach (var methodInfo in methodInfos)
            {
                if (methodInfo.DeclaringType != testClass)
                    continue;

                string methodNameAsText = methodInfo.Name.Replace("_", " ");

                string pattern = @"[A-Z][a-z0-9]*";
                Regex regex = new Regex(pattern);
                var wordMatches = regex.Matches(methodNameAsText);

                methodNameAsText = string.Empty;
                foreach (var word in wordMatches)
                {
                    methodNameAsText += (string.IsNullOrEmpty(methodNameAsText) ? string.Empty : " ") + word;
                }

                Action action = (Action)methodInfo.CreateDelegate(typeof(Action), testClassInstance);
                RunTest(action, methodNameAsText, "PASSED", "FAILED", prepend);
            }
        }
    }
}
