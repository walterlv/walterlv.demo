using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// A test case that is discovered from a unit test method.
    /// A unit test method may have multiple test cases.
    /// </summary>
    internal interface ITestCase
    {
        /// <summary>
        /// Get the display name of this test case.
        /// </summary>
        [NotNull]
        string DisplayName { get; }

        /// <summary>
        /// Get the test result of this test case. This may cause an invoking the action of a test case.
        /// </summary>
        [NotNull]
        TestResult Result { get; }
    }
}
