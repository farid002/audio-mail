using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Recognition;


namespace AudioMail
{

    public partial class NewMail : Window
    {
        SpeechRecognitionEngine speechRecEngine = new SpeechRecognitionEngine();
        public NewMail()
        {
            InitializeComponent();
            
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            speechRecEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        void speechRecEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "say my name":
                    newMailBox.AppendText("Farid\n");
                    break;
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            speechRecEngine.RecognizeAsyncStop();
        }

        private void NewMail_Loaded(object sender, RoutedEventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "open sent", "say my name" });
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);
            Grammar grammar = new Grammar(grammarBuilder);

            speechRecEngine.LoadGrammarAsync(grammar);
            speechRecEngine.SetInputToDefaultAudioDevice();
            speechRecEngine.SpeechRecognized += speechRecEngine_SpeechRecognized;
        }
    }
}
