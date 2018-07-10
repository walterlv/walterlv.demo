using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.AssertExtensions
{
    /// <summary>
    /// A set of assertion libraries that support chained syntax
    /// </summary>
    public static class StringAssertExtensions
    {
        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="stringAssert"></param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to occur within <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="substring"/>
        /// is not in <paramref name="value"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="substring"/> is not found in
        /// <paramref name="value"/>.
        /// </exception>
        public static StringAssert ContainsT(this StringAssert stringAssert, string value, string substring, string message = "")
        {
            StringAssert.Contains(value, substring, message, null);
            return stringAssert;
        }

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and throws an exception if the test string does not start with the
        /// substring.
        /// </summary>
        /// <param name="stringAssert"></param>
        /// <param name="value">
        /// The string that is expected to begin with <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to be a prefix of <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// does not begin with <paramref name="substring"/>. The message is
        /// shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> does not begin with
        /// <paramref name="substring"/>.
        /// </exception>
        public static StringAssert StartsWithT(this StringAssert stringAssert, string value, string substring, string message = "")
        {
            StringAssert.StartsWith(value, substring, message, null);
            return stringAssert;
        }

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and throws an exception if the test string does not end with the
        /// substring.
        /// </summary>
        /// <param name="stringAssert"></param>
        /// <param name="value">
        /// The string that is expected to end with <paramref name="substring"/>.
        /// </param>
        /// <param name="substring">
        /// The string expected to be a suffix of <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// does not end with <paramref name="substring"/>. The message is
        /// shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> does not end with
        /// <paramref name="substring"/>.
        /// </exception>
        public static StringAssert EndsWithT(this StringAssert stringAssert, string value, string substring, string message = "")
        {
            StringAssert.EndsWith(value, substring, message, null);
            return stringAssert;
        }

        /// <summary>
        /// Tests whether the specified string matches a regular expression and
        /// throws an exception if the string does not match the expression.
        /// </summary>
        /// <param name="stringAssert"></param>
        /// <param name="value">
        /// The string that is expected to match <paramref name="pattern"/>.
        /// </param>
        /// <param name="pattern">
        /// The regular expression that <paramref name="value"/> is
        /// expected to match.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// does not match <paramref name="pattern"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> does not match
        /// <paramref name="pattern"/>.
        /// </exception>
        public static StringAssert MatchesT(this StringAssert stringAssert, string value, Regex pattern, string message = "")
        {
            StringAssert.Matches(value, pattern, message, null);
            return stringAssert;
        }

        /// <summary>
        /// Tests whether the specified string does not match a regular expression
        /// and throws an exception if the string matches the expression.
        /// </summary>
        /// <param name="stringAssert"></param>
        /// <param name="value">
        /// The string that is expected not to match <paramref name="pattern"/>.
        /// </param>
        /// <param name="pattern">
        /// The regular expression that <paramref name="value"/> is
        /// expected to not match.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// matches <paramref name="pattern"/>. The message is shown in test
        /// results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> matches <paramref name="pattern"/>.
        /// </exception>
        public static StringAssert DoesNotMatchT(this StringAssert stringAssert, string value, Regex pattern, string message = "")
        {
            StringAssert.DoesNotMatch(value, pattern, message, null);
            return stringAssert;
        }
    }
}
