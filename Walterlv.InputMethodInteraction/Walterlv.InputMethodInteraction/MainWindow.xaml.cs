using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Walterlv.InputMethodInteraction
{
    public partial class MainWindow : Window
    {
        private ImeSupport _imeSupport;

        public MainWindow()
        {
            InitializeComponent();
            PreviewKeyDown += OnPreviewKeyDown;
            PreviewTextInput += OnPreviewTextInput;

            TextCompositionEventHandler previewTextInputStartHandler = OnPreviewTextInputStart;
            TextCompositionEventHandler textInputStartHandler = OnTextInputStart;
            TextCompositionEventHandler previewTextInputUpdateHandler = OnPreviewTextInputUpdate;
            TextCompositionEventHandler textInputUpdateHandler = OnTextInputUpdate;
            TextCompositionEventHandler previewTextInputHandler = OnPreviewTextInput;
            TextCompositionEventHandler textInputHandler = OnTextInput;
            AddHandler(TextCompositionManager.PreviewTextInputStartEvent, previewTextInputStartHandler);
            AddHandler(TextCompositionManager.TextInputStartEvent, textInputStartHandler);
            AddHandler(TextCompositionManager.PreviewTextInputUpdateEvent, previewTextInputUpdateHandler);
            AddHandler(TextCompositionManager.TextInputUpdateEvent, textInputUpdateHandler);
            AddHandler(TextCompositionManager.PreviewTextInputEvent, previewTextInputHandler);
            AddHandler(TextCompositionManager.TextInputEvent, textInputHandler);

            _imeSupport = new ImeSupport(this);
        }

        private void OnPreviewTextInputStart(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInputStart");

            // Keyboard.Focus(DebugTextBox);
        }

        private void OnTextInputStart(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInputStart");
            InputingInputBox.Text = e.TextComposition.CompositionText;
            _imeSupport.UpdateCompositionWindow();
        }

        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInputUpdate");
        }

        private void OnTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInputUpdate");
            InputingInputBox.Text = e.TextComposition.CompositionText;
            _imeSupport.UpdateCompositionWindow();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInput");
        }

        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInput");
            InputingInputBox.Text = "";
            if (e.Text == "\b")
            {
                var text = CustomInputBox.Text;
                if (text.Length > 0)
                {
                    CustomInputBox.Text = text.Substring(0, text.Length - 1);
                }
            }
            else
            {
                CustomInputBox.Text += e.Text;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var pressedKey = $"Key={e.Key}, SysKey={e.SystemKey}, ImeKey={e.ImeProcessedKey}";
            Debug.WriteLine(pressedKey);
            DebugTextBlock.Text = pressedKey;
        }
    }
}
