using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Walterlv.Demo.Annotations;

namespace Walterlv.Demo
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ItemViewModel> Items { get; } = new ObservableCollection<ItemViewModel>();

        public MainViewModel()
        {
            for (var i = 0; i < 1000; i++)
            {
                Items.Add(new ItemViewModel {Value = $"DegofaNira = {i}"});
            }
        }
    }

    public class ItemViewModel : ViewModelBase
    {
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetValue(ref _isSelected, value);
        }

        private string _value;
        private bool _isSelected;
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
