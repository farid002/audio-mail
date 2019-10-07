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
        
        //Record button functions
        private void Record_Button_Click(object sender, RoutedEventArgs e)
        {
            Record_Button.IsEnabled = false;
            Progress_Bar.IsEnabled = true;
            Stop_Button.IsEnabled = true;
            speechRecEngine.RecognizeAsync(RecognizeMode.Multiple);   
            
        }

        //Speech to text engine
        void SpeechRecEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
           
            switch (e.Result.Text)
            {
                case "say my name":
                    newMailBox.AppendText("Farid\n");
                    break;
                case "open sent":
                    newMailBox.AppendText("Josha\n");
                    break;
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            speechRecEngine.RecognizeAsyncStop();
            Record_Button.IsEnabled = true;
            Stop_Button.IsEnabled = false;
            Progress_Bar.IsEnabled = false;

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
            speechRecEngine.SpeechRecognized += SpeechRecEngine_SpeechRecognized;
            
        }
    }
}
