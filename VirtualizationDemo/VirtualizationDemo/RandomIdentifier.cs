using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Walterlv.Algorithm
{
    public class RandomIdentifier
    {
        private readonly Random _random = new Random();

        public string Generate(bool pascal)
        {
            var builder = new StringBuilder();
            var wordCount = _random.Next(2, 4);
            for (var i = 0; i < wordCount; i++)
            {
                var syllableCount = 4 - (int) Math.Sqrt(_random.Next(0, 16));
                for (var j = 0; j < syllableCount; j++)
                {
                    var consonant = Consonants[_random.Next(Consonants.Count)];
                    var vowel = Vowels[_random.Next(Vowels.Count)];
                    if ((pascal || i != 0) && j == 0)
                    {
                        consonant = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(consonant);
                    }

                    builder.Append(consonant);
                    builder.Append(vowel);
                }
            }

            return builder.ToString();
        }

        private static readonly List<string> Consonants = new List<string>
        {
            "q","w","r","t","y","p","s","d","f","g","h","j","k","l","z","x","c","v","b","n","m",
            "w","r","t","p","s","d","f","g","h","j","k","l","c","b","n","m",
            "r","t","p","s","d","h","j","k","l","c","b","n","m",
            "r","t","s","j","c","n","m",
            "tr","dr","ch","wh","st",
            "s","s"
        };

        private static readonly List<string> Vowels = new List<string>
        {
            "a","e","i","o","u",
            "a","e","i","o","u",
            "a","e","i",
            "a","e",
            "e",
            "ar","as","ai","air","ay","al","all","aw",
            "ee","ea","ear","em","er","el","ere",
            "is","ir",
            "ou","or","oo","ou","ow",
            "ur"
        };
    }
}
