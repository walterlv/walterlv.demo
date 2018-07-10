using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.AssertExtensions
{
    /// <summary>
    /// A set of assertion libraries that support chained syntax
    /// </summary>
    public static class AssertExtensions
    {
        /// <summary>
        /// Tests whether the specified object is an instance of the expected
        /// type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="value">
        /// The object the test expects to be of the specified type.
        /// </param>
        /// <typeparam name="T">
        /// The expected type of <paramref name="value"/>.
        /// </typeparam>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not an instance of <typeparam name="T"/>. The message is
        /// shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is null or
        /// <typeparam name="T"/> is not in the inheritance hierarchy
        /// of <paramref name="value"/>.
        /// </exception>
        public static Assert IsInstanceOfType<T>(this Assert assert, object value, string message = "")
        {
            Assert.IsInstanceOfType(value, typeof(T));
            return assert;
        }

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong
        /// type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="value">
        /// The object the test expects not to be of the specified type.
        /// </param>
        /// <typeparam name="T">
        /// The type that <paramref name="value"/> should not be.
        /// </typeparam>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is an instance of <typeparam name="T"/>. The message is shown
        /// in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is not null and
        /// <typeparam name="T"/> is in the inheritance hierarchy
        /// of <paramref name="value"/>.
        /// </exception>
        public static Assert IsNotInstanceOfType<T>(this Assert assert, object value, string message = "")
        {
            Assert.IsNotInstanceOfType(value, typeof(T), message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified object is null and throws an exception
        /// if it is not.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="value">
        /// The object the test expects to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not null. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is not null.
        /// </exception>
        public static Assert IsNullT(this Assert assert, [CanBeNull]object value, string message = "")
        {
            Assert.IsNull(value);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception
        /// if the condition is false.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="condition">
        /// The condition the test expects to be true.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is false. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="condition"/> is false.
        /// </exception>
        public static Assert IsTrueT(this Assert assert, bool condition, string message = "")
        {
            Assert.IsTrue(condition, message);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception
        /// if the condition is true.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="condition">
        /// The condition the test expects to be false.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is true. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="condition"/> is true.
        /// </exception>
        public static Assert IsFalseT(this Assert assert, bool condition, string message = "")
        {
            Assert.IsFalse(condition, message);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception
        /// if it is null.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="value">
        /// The object the test expects not to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is null. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is null.
        /// </exception>
        public static Assert IsNotNullT(this Assert assert, [CanBeNull]object value, string message = "")
        {
            Assert.IsNotNull(value, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and
        /// throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="expected">
        /// The first object to compare. This is the value the test expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not the same as <paramref name="expected"/>. The message is shown
        /// in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> does not refer to the same object
        /// as <paramref name="actual"/>.
        /// </exception>
        public static Assert AreSameT(this Assert assert, object expected, object actual, string message = "")
        {
            Assert.AreSame(expected, actual, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified objects refer to different objects and
        /// throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is the same as <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> refers to the same object
        /// as <paramref name="actual"/>.
        /// </exception>
        public static Assert AreNotSameT(this Assert assert, object notExpected, object actual, string message)
        {
            Assert.AreNotSame(notExpected, actual, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception
        /// if the two values are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="assert"></param>
        /// <param name="expected">
        /// The first value to compare. This is the value the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to
        /// <paramref name="actual"/>.
        /// </exception>
        public static Assert AreEqualT<T>(this Assert assert, T expected, T actual, string message = "")
        {
            Assert.AreEqual(expected, actual, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception
        /// if the two values are equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="assert"></param>
        /// <param name="notExpected">
        /// The first value to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static Assert AreNotEqualT<T>(this Assert assert, T notExpected, T actual, string message = "")
        {
            Assert.AreNotEqual(notExpected, actual, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception
        /// if they are not equal.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="expected">
        /// The first double to compare. This is the double the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="expected"/>
        /// by more than <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is different than <paramref name="expected"/> by more than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static Assert AreEqualT(this Assert assert, double expected, double actual, double delta, string message = "")
        {
            Assert.AreEqual(expected, actual, delta, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception
        /// if they are equal.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="notExpected">
        /// The first double to compare. This is the double the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="notExpected"/>
        /// by at most <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/> or different by less than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static Assert AreNotEqualT(this Assert assert, double notExpected, double actual, double delta, string message = "")
        {
            Assert.AreNotEqual(notExpected, actual, delta, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception
        /// if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="expected">
        /// The first string to compare. This is the string the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static Assert AreEqualT(this Assert assert, string expected, string actual, bool ignoreCase, string message = "")
        {
            Assert.AreEqual(expected, actual, ignoreCase, message, null);
            return assert;
        }

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception
        /// if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="notExpected">
        /// The first string to compare. This is the string the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static Assert AreNotEqualT(this Assert assert, string notExpected, string actual, bool ignoreCase, string message = "")
        {
            Assert.AreNotEqual(notExpected, actual, ignoreCase, message, null);
            return assert;
        }
    }
}
