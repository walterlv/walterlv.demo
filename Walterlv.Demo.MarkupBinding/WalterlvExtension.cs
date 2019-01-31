using System;
using System.Windows.Markup;

namespace Walterlv.Demo.MarkupBinding
{
    public class WalterlvExtension : MarkupExtension
    {
        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged(value);
            }
        }

        private void OnValueChanged(object newValue)
        {
            
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}