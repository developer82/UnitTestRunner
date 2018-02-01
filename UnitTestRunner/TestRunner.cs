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
            Console.Write($"{testText}...\t\t");

            bool testPassed = RunTest(testMethod);
            if (testPassed)
            {
                ColorConsole.WithGreenText.WriteLine($"[{passText}]");
            }
            else
            {
                ColorConsole.WithRedText.WriteLine($"[{failText}]");
            }

            return testPassed;
        }

        public void RunTestClass(Type testClass)
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
                RunTest(action, methodNameAsText, "PASSED", "FAILED");
            }
        }
    }
}
