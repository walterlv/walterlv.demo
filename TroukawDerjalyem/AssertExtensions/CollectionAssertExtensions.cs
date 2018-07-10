using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.AssertExtensions
{
    /// <summary>
    /// A set of assertion libraries that support chained syntax
    /// </summary>
    public static class CollectionAssertExtensions
    {
        /// <summary>
        /// Tests whether the specified collection contains the specified element
        /// and throws an exception if the element is not in the collection.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected to be in the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="element"/>
        /// is not in <paramref name="collection"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="element"/> is not found in
        /// <paramref name="collection"/>.
        /// </exception>
        public static CollectionAssert ContainsT(this CollectionAssert collectionAssert, ICollection collection, object element, string message = "")
        {
            CollectionAssert.Contains(collection, element, string.Empty, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether the specified collection does not contain the specified
        /// element and throws an exception if the element is in the collection.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected not to be in the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="element"/>
        /// is in <paramref name="collection"/>. The message is shown in test
        /// results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="element"/> is found in
        /// <paramref name="collection"/>.
        /// </exception>
        public static CollectionAssert DoesNotContainT(this CollectionAssert collectionAssert, ICollection collection, object element, string message = "")
        {
            CollectionAssert.DoesNotContain(collection, element, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether all items in the specified collection are non-null and throws
        /// an exception if any element is null.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="collection">
        /// The collection in which to search for null elements.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="collection"/>
        /// contains a null element. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if a null element is found in <paramref name="collection"/>.
        /// </exception>
        public static CollectionAssert AllItemsAreNotNullT(this CollectionAssert collectionAssert, ICollection collection, string message = "")
        {
            CollectionAssert.AllItemsAreNotNull(collection, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and
        /// throws if any two elements in the collection are equal.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="collection">
        /// The collection in which to search for duplicate elements.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="collection"/>
        /// contains at least one duplicate element. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if a two or more equal elements are found in
        /// <paramref name="collection"/>.
        /// </exception>
        public static CollectionAssert AllItemsAreUniqueT(this CollectionAssert collectionAssert, ICollection collection, string message = "")
        {
            CollectionAssert.AllItemsAreUnique(collection, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether one collection is a subset of another collection and
        /// throws an exception if any element in the subset is not also in the
        /// superset.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="subset">
        /// The collection expected to be a subset of <paramref name="superset"/>.
        /// </param>
        /// <param name="superset">
        /// The collection expected to be a superset of <paramref name="subset"/>
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when an element in
        /// <paramref name="subset"/> is not found in <paramref name="superset"/>.
        /// The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if an element in <paramref name="subset"/> is not found in
        /// <paramref name="superset"/>.
        /// </exception>
        public static CollectionAssert IsSubsetOfT(this CollectionAssert collectionAssert, ICollection subset, ICollection superset, string message = "")
        {
            CollectionAssert.IsSubsetOf(subset, superset, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether one collection is not a subset of another collection and
        /// throws an exception if all elements in the subset are also in the
        /// superset.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="subset">
        /// The collection expected not to be a subset of <paramref name="superset"/>.
        /// </param>
        /// <param name="superset">
        /// The collection expected not to be a superset of <paramref name="subset"/>
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when every element in
        /// <paramref name="subset"/> is also found in <paramref name="superset"/>.
        /// The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if every element in <paramref name="subset"/> is also found in
        /// <paramref name="superset"/>.
        /// </exception>
        public static CollectionAssert IsNotSubsetOfT(this CollectionAssert collectionAssert, ICollection subset, ICollection superset, string message = "")
        {
            CollectionAssert.IsNotSubsetOf(subset, superset, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether two collections contain the same elements and throws an
        /// exception if either collection contains an element not in the other
        /// collection.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="expected">
        /// The first collection to compare. This contains the elements the test
        /// expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by
        /// the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when an element was found
        /// in one of the collections but not the other. The message is shown
        /// in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if an element was found in one of the collections but not
        /// the other.
        /// </exception>
        public static CollectionAssert AreEquivalentT(this CollectionAssert collectionAssert, ICollection expected, ICollection actual, string message = "")
        {
            CollectionAssert.AreEquivalent(expected, actual, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether two collections contain the different elements and throws an
        /// exception if the two collections contain identical elements without regard
        /// to order.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="expected">
        /// The first collection to compare. This contains the elements the test
        /// expects to be different than the actual collection.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by
        /// the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// contains the same elements as <paramref name="expected"/>. The message
        /// is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if the two collections contained the same elements, including
        /// the same number of duplicate occurrences of each element.
        /// </exception>
        public static CollectionAssert AreNotEquivalentT(this CollectionAssert collectionAssert, ICollection expected, ICollection actual, string message = "")
        {
            CollectionAssert.AreNotEquivalent(expected, actual, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether all elements in the specified collection are instances
        /// of the expected type and throws an exception if the expected type is
        /// not in the inheritance hierarchy of one or more of the elements.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="collection">
        /// The collection containing elements the test expects to be of the
        /// specified type.
        /// </param>
        /// <param name="expectedType">
        /// The expected type of each element of <paramref name="collection"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when an element in
        /// <paramref name="collection"/> is not an instance of
        /// <paramref name="expectedType"/>. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if an element in <paramref name="collection"/> is null or
        /// <paramref name="expectedType"/> is not in the inheritance hierarchy
        /// of an element in <paramref name="collection"/>.
        /// </exception>
        public static CollectionAssert AllItemsAreInstancesOfTypeT(this CollectionAssert collectionAssert, ICollection collection, Type expectedType, string message = "")
        {
            CollectionAssert.AllItemsAreInstancesOfType(collection, expectedType, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
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
        public static CollectionAssert AreEqualT(this CollectionAssert collectionAssert, ICollection expected, ICollection actual, string message = "")
        {
            CollectionAssert.AreEqual(expected, actual, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static CollectionAssert AreNotEqualT(this CollectionAssert collectionAssert, ICollection notExpected, ICollection actual, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected, actual, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
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
        public static CollectionAssert AreEqualT(this CollectionAssert collectionAssert, ICollection expected, ICollection actual, IComparer comparer, string message = "")
        {
            CollectionAssert.AreEqual(expected, actual, comparer, message, null);
            return collectionAssert;
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="collectionAssert"></param>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static CollectionAssert AreNotEqualT(this CollectionAssert collectionAssert, ICollection notExpected, ICollection actual, IComparer comparer, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected, actual, comparer, message, null);
            return collectionAssert;
        }
    }
}
