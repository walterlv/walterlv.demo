using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using MSTest.Extensions.Core;

namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// Contains methods to write contract based unit test code.
    /// </summary>
    public static partial class ContractTest
    {
        #region Style 1： "Contract".Test(() => { Test Case Code })

        /// <summary>
        /// Create a test case for the specified <paramref name="contract"/>.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The action of the which is used to test the contract.</param>
        [PublicAPI]
        public static void Test([NotNull] this string contract, [NotNull] Action testCase)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));
            Contract.EndContractBlock();

            Method.Current.Add(new ContractTestCase(contract, testCase));
        }

        /// <summary>
        /// Create an async test case for the specified <paramref name="contract"/>.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The async action of the which is used to test the contract.</param>
        [PublicAPI]
        public static void Test([NotNull] this string contract, [NotNull] Func<Task> testCase)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));
            Contract.EndContractBlock();

            Method.Current.Add(new ContractTestCase(contract, testCase));
        }

        #endregion

        #region Style 2： await "Contract" Test Case Code

        ///// <summary>
        ///// Treat a string as test case contract description, and then treat the code below await as test case action.
        ///// </summary>
        ///// <param name="contract">The description of a test contract.</param>
        ///// <returns></returns>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static IAwaiter GetAwaiter(this string contract)
        //{
        //    var result = new TestCaseAwaitable(contract);
        //    Method.Current.Add(result);
        //    return result;
        //}

        #endregion

        /// <summary>
        /// Gets all test case information that is collected or will be collected from the test method.
        /// </summary>
        [NotNull]
        internal static TestCaseIndexer Method { get; } = new TestCaseIndexer();
    }
}
