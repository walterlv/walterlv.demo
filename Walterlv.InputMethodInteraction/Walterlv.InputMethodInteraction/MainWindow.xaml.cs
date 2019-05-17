using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Walterlv.InputMethodInteraction
{
    public partial class MainWindow : Window
    {
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
        }

        private void OnPreviewTextInputStart(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInputStart");
            DebugTextBox.Focus();
        }

        private void OnTextInputStart(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInputStart");
        }

        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInputUpdate");
        }

        private void OnTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInputUpdate");
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnPreviewTextInput");
            var textInput = $"Text={e.Text}, SysText={e.SystemText}, CtlText={e.ControlText}";
            Debug.WriteLine(textInput);
            Debug2TextBlock.Text = textInput;
        }

        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"OnTextInput");
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var pressedKey = $"Key={e.Key}, SysKey={e.SystemKey}, ImeKey={e.ImeProcessedKey}";
            Debug.WriteLine(pressedKey);
            DebugTextBlock.Text = pressedKey;
        }
    }
}
