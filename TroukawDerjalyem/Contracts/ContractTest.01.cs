using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace MSTest.Extensions.Contracts
{
    public static partial class ContractTest
    {
        /// <summary>
        /// Create a test case for the specified <paramref name="contract"/>.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The action which is used to test the contract.</param>
        [System.Diagnostics.Contracts.Pure, NotNull, PublicAPI]
        public static ContractTestContext<T> Test<T>([NotNull] this string contract, [NotNull] Action<T> testCase)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            Contract.EndContractBlock();

            return new ContractTestContext<T>(contract, testCase);
        }

        /// <summary>
        /// Create an async test case for the specified <paramref name="contract"/>.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The async action which is used to test the contract.</param>
        [System.Diagnostics.Contracts.Pure, NotNull, PublicAPI]
        public static ContractTestContext<T> Test<T>([NotNull] this string contract, [NotNull] Func<T, Task> testCase)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            Contract.EndContractBlock();

            return new ContractTestContext<T>(contract, testCase);
        }
    }
}
