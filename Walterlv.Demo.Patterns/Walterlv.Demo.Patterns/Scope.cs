using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Walterlv.ERMail.OAuth
{
    /// <summary>
    /// Define some types of permissions that OAuth called Scope.
    /// </summary>
    public class Scope : IEquatable<Scope>, IEnumerable<string>
    {
        /// <summary>
        /// Indicate that this is an invalid scope.
        /// </summary>
        public static Scope Invalid { get; } = new Scope();

        /// <summary>
        /// Gets a value that indicates whether this scope is invalid.
        /// </summary>
        public bool IsInvalid => !_scopes.Any();

        /// <summary>
        /// Initialize a new invalid instance of <see cref="Scope"/>.
        /// </summary>
        private Scope()
        {
        }

        private Scope(IEnumerable<string> scopes)
        {
            if (scopes == null) throw new ArgumentNullException(nameof(scopes));
            _scopes.AddRange(scopes.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Scope"/> with some specified scope types.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="otherScopes"></param>
        public Scope(string scope, params string[] otherScopes)
            : this(scope, otherScopes ?? Enumerable.Empty<string>())
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Scope"/> with some specified scope types.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="otherScopes"></param>
        private Scope(string scope, IEnumerable<string> otherScopes)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            if (otherScopes == null) throw new ArgumentNullException(nameof(otherScopes));

            var parts = scope.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 0)
            {
                throw new ArgumentException("Specified scope string doesnot contains any scope values.", nameof(scope));
            }

            _scopes.AddRange(parts.Distinct());
            _scopes.AddRange(otherScopes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .SelectMany(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Distinct());
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Scope"/> that merge two scope collections.
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="otherScopes"></param>
        private Scope(IEnumerable<string> scopes, IEnumerable<string> otherScopes)
        {
            _scopes.AddRange(scopes.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
            foreach (var scope in otherScopes.Where(x => !string.IsNullOrWhiteSpace(x))
                .Where(x => !_scopes.Contains(x)))
            {
                _scopes.Add(scope);
            }
        }

        private readonly List<string> _scopes = new List<string>();

        /// <inheritdoc />
        public bool Equals(Scope other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_scopes.Count != other._scopes.Count) return false;
            if (_scopes.Except(other._scopes).Any()) return false;
            return true;
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var scope in _scopes)
            {
                yield return scope;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Scope)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var result = 0;

            foreach (var scope in _scopes)
            {
                result ^= scope.GetHashCode();
            }

            return result;
        }

        public override string ToString()
        {
            return string.Join(" ", _scopes.OrderBy(x => x));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(Scope scope1, Scope scope2)
        {
            return Equals(scope1, scope2);
        }

        public static bool operator !=(Scope scope1, Scope scope2)
        {
            return !Equals(scope1, scope2);
        }

        /// <summary>
        /// Merge two scopes and returns a new scope with all their scopes.
        /// </summary>
        public static Scope operator |(Scope scope1, Scope scope2)
        {
            return new Scope(scope1._scopes, scope2._scopes);
        }

        /// <summary>
        /// Merge two scopes and returns a new scope with all their scopes.
        /// </summary>
        public static Scope operator |(string scope1, Scope scope2)
        {
            return new Scope(scope1, scope2._scopes);
        }

        /// <summary>
        /// Merge two scopes and returns a new scope with all their scopes.
        /// </summary>
        public static Scope operator |(Scope scope1, string scope2)
        {
            return new Scope(scope2, scope1._scopes);
        }

        /// <summary>
        /// Findout the common scopes between two scopes and returns a new <see cref="Scope"/> to indicate it.
        /// If all their permissions are different, it will returns an invalid one.
        /// </summary>
        public static Scope operator &(Scope scope1, Scope scope2)
        {
            return new Scope(scope1._scopes.Intersect(scope2._scopes));
        }

        /// <summary>
        /// Findout the common scopes between two scopes and returns a new <see cref="Scope"/> to indicate it.
        /// If all their permissions are different, it will returns an invalid one.
        /// </summary>
        public static Scope operator &(string scope1, Scope scope2)
        {
            if (scope2._scopes.Contains(scope1))
            {
                return new Scope(scope1);
            }

            return Invalid;
        }

        /// <summary>
        /// Findout the common scopes between two scopes and returns a new <see cref="Scope"/> to indicate it.
        /// If all their permissions are different, it will returns an invalid one.
        /// </summary>
        public static Scope operator &(Scope scope1, string scope2)
        {
            if (scope1._scopes.Contains(scope2))
            {
                return new Scope(scope2);
            }

            return Invalid;
        }

        public static implicit operator Scope(string scope)
        {
            return new Scope(scope);
        }

        public static implicit operator Scope(string[] scope)
        {
            return new Scope(scope);
        }

        public static implicit operator string[] (Scope scope)
        {
            return scope._scopes.ToArray();
        }
    }
}
