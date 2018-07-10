using System.Reflection;

namespace MSTest.Extensions.Utils
{
    /// <summary>
    /// object extension.used for getting/setting both public and nonpublic property or field of an instance
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// used for getting both public and nonpublic field of an instance
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetField([NotNull] this object source, string fieldName)
        {
            var type = source.GetType();
            var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(source);
        }

        /// <summary>
        /// used for getting both public and nonpublic property of an instance
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty([NotNull] this object source, string propertyName)
        {
            var type = source.GetType();
            var property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return property.GetValue(source);
        }

        /// <summary>
        /// used for setting both public and nonpublic field of an instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetField([NotNull] this object target, string fieldName, object value)
        {
            var type = target.GetType();
            var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, value);
        }

        /// <summary>
        /// used for getting both public and nonpublic property of an instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetProperty([NotNull] this object target, string propertyName, object value)
        {
            var type = target.GetType();
            var property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(target, value);
        }
    }
}
