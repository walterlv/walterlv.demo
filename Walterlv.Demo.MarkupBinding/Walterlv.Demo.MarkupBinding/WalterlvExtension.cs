using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Walterlv.Demo.MarkupBinding
{
    public class WalterlvExtension : MarkupExtension
    {
        public WalterlvExtension()
        {
            _valueExchanger = new ClrBindingExchanger(this, ValueProperty, OnValueChanged);
        }

        private readonly ClrBindingExchanger _valueExchanger;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value", typeof(object), typeof(WalterlvExtension),
            new PropertyMetadata(null, ClrBindingExchanger.ValueChangeCallback));

        [Bindable(true)]
        public object Value
        {
            get => _valueExchanger.GetValue();
            set => _valueExchanger.SetValue(value);
        }

        private void OnValueChanged(object oldValue, object newValue)
        {
            // 在这里可以处理 Value 属性值改变的变更通知。
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }

    internal class ClrBindingExchanger : DependencyObject
    {
        private readonly object _owner;
        private readonly DependencyProperty _attachedProperty;
        private readonly Action<object, object> _valueChangeCallback;

        public ClrBindingExchanger(object owner, DependencyProperty attachedProperty,
            Action<object, object> valueChangeCallback = null)
        {
            _owner = owner;
            _attachedProperty = attachedProperty;
            _valueChangeCallback = valueChangeCallback;
        }

        public object GetValue()
        {
            return GetValue(_attachedProperty);
        }

        public void SetValue(object value)
        {
            if (value is Binding binding)
            {
                BindingOperations.SetBinding(this, _attachedProperty, binding);
            }
            else
            {
                SetValue(_attachedProperty, value);
            }
        }

        public static void ValueChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ClrBindingExchanger) d)._valueChangeCallback?.Invoke(e.OldValue, e.NewValue);
        }
    }
}