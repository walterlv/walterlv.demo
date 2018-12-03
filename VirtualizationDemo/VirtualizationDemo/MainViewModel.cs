using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Walterlv.Algorithm;
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
                Items.Add(new ItemViewModel($"{_identifier.Generate(i % 2 == 0)} = {i}"));
            }
        }

        private readonly RandomIdentifier _identifier = new RandomIdentifier();
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
