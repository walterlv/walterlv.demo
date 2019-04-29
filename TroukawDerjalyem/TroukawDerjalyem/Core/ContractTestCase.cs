using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Core
{
    /// <inheritdoc />
    /// <summary>
    /// A contract based test case that is discovered from a unit test method.
    /// </summary>
    internal class ContractTestCase : ITestCase
    {
        /// <summary>
        /// Create a new instance of <see cref="ContractTestCase"/> with contract description and an action.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The action of the which is used to test the contract.</param>
        internal ContractTestCase([NotNull] string contract, [NotNull] Action testCase)
        {
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));
            _contract = contract ?? throw new ArgumentNullException(nameof(contract));
#if NET45
#pragma warning disable CS1998 // Async function without await expression
            _testCase = async () => testCase();
#pragma warning restore CS1998 // Async function without await expression
#else
            _testCase = () =>
            {
                testCase();
                return Task.CompletedTask;
            };
#endif
        }

        /// <summary>
        /// Create a new instance of <see cref="ContractTestCase"/> with contract description and an async action.
        /// </summary>
        /// <param name="contract">The description of a test contract.</param>
        /// <param name="testCase">The action of the which is used to test the contract.</param>
        internal ContractTestCase([NotNull] string contract, [NotNull] Func<Task> testCase)
        {
            _contract = contract ?? throw new ArgumentNullException(nameof(contract));
            _testCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
        }

        /// <inheritdoc />
        public string DisplayName => _contract;

        /// <inheritdoc />
        public TestResult Result => _result ?? (_result = Execute());

        /// <summary>
        /// Invoke the test case action to get the test result.
        /// </summary>
        /// <returns>The test result of this test case.</returns>
        [NotNull]
        private TestResult Execute()
        {
            // Execute the unit test.
            var (duration, output, error, exception) = ExecuteCore();

            // Return the test result.
            var result = new TestResult
            {
                DisplayName = _contract,
                Duration = duration,
                LogOutput = output,
                LogError = error,
            };
            if (exception == null)
            {
                result.Outcome = UnitTestOutcome.Passed;
            }
            else
            {
                result.Outcome = UnitTestOutcome.Failed;
                result.TestFailureException = exception;
            }

            return result;
        }

        /// <summary>
        /// Execute the unit test and get the execution result.
        /// </summary>
        /// <returns>
        /// duration: The ellapsed time of executing this test case.
        /// output: When `Console.Write` is called, the output will be collected into this property.
        /// output: When `Console.Write` is called, it will be collected into this property.
        /// exception: If any exception occurred, it will be collected into this property; otherwise, it will be null.
        /// </returns>
        private (TimeSpan duration, string output, string error, Exception exception) ExecuteCore()
        {
            TimeSpan duration;
            string output;
            string error;
            Exception exception;

            using (var outputWriter = new ThreadSafeStringWriter(CultureInfo.InvariantCulture))
            {
                using (var errorWriter = new ThreadSafeStringWriter(CultureInfo.InvariantCulture))
                {
                    // Prepare the execution.
                    var originOut = Console.Out;
                    var originError = Console.Error;
                    Console.SetOut(outputWriter);
                    Console.SetError(errorWriter);

                    var watch = new Stopwatch();
                    watch.Start();

                    // Execute the test case.
                    try
                    {
                        _testCase().Wait();
                        exception = null;
                    }
                    catch (AggregateException ex)
                    {
                        // If this test case is an async method, extract the inner excetion.
                        exception = ex.InnerExceptions.Count == 1 ? ex.InnerException : ex;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        watch.Stop();
                        Console.SetOut(originOut);
                        Console.SetError(originError);

                        duration = watch.Elapsed;
                        output = outputWriter.ToString();
                        error = errorWriter.ToString();
                    }
                }
            }

            return (duration, output, error, exception);
        }

        [CanBeNull, ContractPublicPropertyName(nameof(Result))]
        private TestResult _result;

        /// <summary>
        /// Gets the action of this test case. Invoke this func will execute the test case.
        /// The returning value of this func will never be null, but it does not mean that it is an async func.
        /// </summary>
        [NotNull] private readonly Func<Task> _testCase;

        /// <summary>
        /// Gets the contract description of this test case.
        /// </summary>
        [NotNull] private readonly string _contract;
    }
}
