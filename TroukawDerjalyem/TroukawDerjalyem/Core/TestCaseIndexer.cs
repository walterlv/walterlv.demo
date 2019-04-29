using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// Contains all test case information of all test unit method which are discovered by <see cref="ContractTestCaseAttribute"/>.
    /// </summary>
    internal class TestCaseIndexer
    {
        /// <summary>
        /// Gets all test cases of a specified method.
        /// </summary>
        /// <param name="method">The target unit test method.</param>
        /// <returns>
        /// The test case list of the specified unit test method. If the discovery is not started, then it returns an empty list.
        /// </returns>
        [NotNull]
        internal IList<ITestCase> this[[NotNull] MethodInfo method]
        {
            get
            {
                if (method == null) throw new ArgumentNullException(nameof(method));
                Contract.EndContractBlock();

                return this[GetKey(method)];
            }
        }

        /// <summary>
        /// Gets all test cases of current unit test method. This method is found through the stack trace.
        /// </summary>
        [NotNull]
        internal IList<ITestCase> Current => this[GetCurrentTestMethod()];

        /// <summary>
        /// Gets all test cases of a specified method key.
        /// </summary>
        /// <param name="testKey">A unique string that indicates a unit test method.</param>
        /// <returns>
        /// The test case list of the specified unit test method. If the discovery is not started, then it returns an empty list.
        /// </returns>
        [NotNull]
        private IList<ITestCase> this[[NotNull] string testKey]
        {
            get
            {
                Contract.EndContractBlock();

                if (!_testCaseDictionary.TryGetValue(testKey, out var list))
                {
                    list = new List<ITestCase>();
                    _testCaseDictionary[testKey] = list;
                }

                return list;
            }
        }

        /// <summary>
        /// Stores all the test cases that discovered or will be discovered from all unit test methods.
        /// Key: namespace.class.method. <see cref="GetKey"/> can generate it correctly.
        /// Value: All test cases of a specified method. You can get rid of null value by calling Indexer.
        /// </summary>
        [NotNull] private readonly Dictionary<string, List<ITestCase>> _testCaseDictionary =
            new Dictionary<string, List<ITestCase>>();

        /// <summary>
        /// Get the unique string that indicates a unit test method.
        /// </summary>
        /// <param name="member">The unit test method.</param>
        /// <returns>The unique string that indicates a unit test method.</returns>
        [NotNull, SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static string GetKey([NotNull] MemberInfo member)
        {
            var type = member.DeclaringType;
            Contract.Requires(type != null);
            Contract.EndContractBlock();

            return $"{type.FullName}.{member.Name}";
        }

        /// <summary>
        /// Find the unit test method through current stack trace.
        /// </summary>
        /// <returns>The unit test method that found from stack trace.</returns>
        [NotNull]
        private static MethodInfo GetCurrentTestMethod()
        {
            var stackTrace = new StackTrace();
            for (var i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                if (method.GetCustomAttribute<TestMethodAttribute>() != null)
                {
                    return (MethodInfo) method;
                }
            }

            throw new InvalidOperationException(
                "There is no unit test method in the current stack trace. " +
                "This method should only be called directly or indirectly from the unit test method.");
        }
    }
}
