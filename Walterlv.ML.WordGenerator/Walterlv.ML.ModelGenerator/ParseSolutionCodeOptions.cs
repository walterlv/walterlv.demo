using dotnetCampus.Cli;
using PlatformSpellCheck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class ParseSolutionCodeOptions
    {
        [Option('f', "CodeFolder")]
        public string CodeFolder { get; set; }

        [Option('o', "OutputFile")]
        public string OutputFile { get; set; }

        public async Task RunAsync()
        {
            var directory = new DirectoryInfo(CodeFolder);
            var outputFile = new FileInfo(string.IsNullOrWhiteSpace(OutputFile) ? "output.txt" : OutputFile);
            await GenerateFromSolutionCodeAsync(directory, outputFile).ConfigureAwait(false);
        }

        private async Task GenerateFromSolutionCodeAsync(DirectoryInfo directory, FileInfo outputFile)
        {
            var wordBag = new Dictionary<string, bool>();
            var seperators = new char[] { ' ', '\r', '\n', '>', '<', '.', ';', '?' };
            var wordRegex = new Regex(@"([A-Za-z]+)", RegexOptions.Compiled);
            var caseRegex = new Regex(@"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", RegexOptions.Compiled);

            var spell = new SpellChecker("en-us");

            var count = 0;
            foreach (var file in directory.EnumerateFiles("*.cs", SearchOption.AllDirectories))
            {
                var text = File.ReadAllText(file.FullName);
                var matches = wordRegex.Matches(text);
                foreach (Match match in matches)
                {
                    var words = caseRegex.Replace(match.Value, " ");
                    var splittedwords = words.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in splittedwords)
                    {
                        var w = word.ToLower(CultureInfo.InvariantCulture);
                        if (!wordBag.ContainsKey(w))
                        {
                            count++;
                            var hasError = spell.Check(w).Any();
                            if (hasError)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine($"[{count.ToString().PadLeft(5, ' ')}] {word.PadRight(32, ' ')} {file.Name}");
                            Console.ResetColor();
                            wordBag[w] = !hasError;
                        }
                    }
                }
            }

            var builder = new StringBuilder();
            builder.AppendLine($"Label\tValue");
            foreach (var word in wordBag)
            {
                //var formatted = Format(word.Key, 10);
                //if (string.IsNullOrWhiteSpace(formatted))
                //{
                //    continue;
                //}
                builder.AppendLine($"{(word.Value ? "1" : "0")}\t{word.Key}");
            }

            File.WriteAllText(outputFile.FullName, builder.ToString());

            var spellBuilder = new StringBuilder();
            foreach (var word in wordBag)
            {
                spellBuilder.AppendLine(word.Key);
            }

            File.WriteAllText(Path.Combine(outputFile.DirectoryName, "spell-" + outputFile.Name), spellBuilder.ToString());
        }

        private string Format(string input, int limit)
        {
            var builder = new StringBuilder();
            if (input.Length > limit)
                return string.Empty;
            builder.Append(input);
            builder.Append(' ');
            //for (int i = 0; i < limit; i++)
            //{
            //    var index = i;
            //    if (input.Length <= index)
            //    {
            //        builder.Append('0');
            //    }
            //    else
            //    {
            //        builder.Append(input[index]);
            //    }
            //    builder.Append(' ');
            //}
            for (int i = 0; i < limit ; i++)
            {
                var index = i;
                if (input.Length < index + 1)
                {
                    builder.Append('0');
                    builder.Append('0');
                }
                else if (input.Length == index + 1)
                {
                    builder.Append(input[index]);
                    builder.Append('0');
                }
                else
                {
                    builder.Append(input[index]);
                    builder.Append(input[index + 1]);
                }
                builder.Append(' ');
            }
            builder.Length--;

            //for (int i = limit; i < limit+limit - 2; i++)
            //{
            //    var index = i - limit;
            //    if (input.Length < index + 1)
            //    {
            //        builder.Append('0');
            //        builder.Append('0');
            //    }
            //    else if (input.Length == index + 1)
            //    {
            //        builder.Append(input[index]);
            //        builder.Append('0');
            //    }
            //    else
            //    {
            //        builder.Append(input[index]);
            //        builder.Append(input[index + 1]);
            //    }
            //    builder.Append(' ');
            //}
            //builder.Length--;
            return builder.ToString();
        }
    }
}
