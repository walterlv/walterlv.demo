using System.Collections.Generic;

namespace Walterlv.Framework
{
    internal static class OptionParserUtility
    {
        public static T Parse<T>(this ICommandLineOptionParser<T> parser,
            Dictionary<string, IReadOnlyList<string>> options)
        {
            foreach (var optionValue in options)
            {
                var option = optionValue.Key;
                var values = optionValue.Value;
                if (string.IsNullOrWhiteSpace(option))
                {

                }
            }

            return default;
        }
    }
}