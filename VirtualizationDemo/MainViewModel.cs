using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Walterlv.Demo.Annotations;

namespace Walterlv.Demo
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ItemViewModel> SelectedItems { get; } = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items { get; } = new ObservableCollection<ItemViewModel>();

        public MainViewModel()
        {
            for (var i = 0; i < 1000; i++)
            {
                Items.Add(new ItemViewModel($"{GenerateIdentifier()} = {i}"));
            }
        }

private static readonly Random Random = new Random();

private static string GenerateIdentifier()
{
    var builder = new StringBuilder();
    var wordCount = Random.Next(2, 4);
    for (var i = 0; i < wordCount; i++)
    {
        var syllableCount = 4 - (int) Math.Sqrt(Random.Next(1, 15));
        for (var j = 0; j < syllableCount; j++)
        {
            var consonant = Consonants[Random.Next(Consonants.Count)];
            var vowel = Vowels[Random.Next(Vowels.Count)];
            if (j == 0)
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

    public class ItemViewModel : ViewModelBase, IEquatable<ItemViewModel>
    {
        public ItemViewModel(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetValue(ref _isSelected, value);
        }

        private bool _isSelected;

        public bool Equals(ItemViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemViewModel) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
