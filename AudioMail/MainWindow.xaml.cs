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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Cloud.Speech.V1;
using System.Speech.Recognition;


namespace AudioMail
{
    
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine speechRecMain = new SpeechRecognitionEngine();
        
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Choices commands = new Choices();
            //commands.Add(new string[] { "compose new mail", "open recieved mails", "open deleted mails", "open starred mails" });
            //GrammarBuilder grammarBuilder = new GrammarBuilder();
            //grammarBuilder.Append(commands);
            //Grammar grammar = new Grammar(grammarBuilder);
            DictationGrammar grammar = new DictationGrammar();
            speechRecMain.LoadGrammarAsync(grammar);
            speechRecMain.SetInputToDefaultAudioDevice();
            speechRecMain.SpeechRecognized += speechRecMain_SpeechRecognized;


        }

        //Record button functions
        private void Record_Button_Click(object sender, RoutedEventArgs e)
        {
            Record_Button.IsEnabled = false;
            Progress_Bar.IsEnabled = true;
            Stop_Button.IsEnabled = true;
            speechRecMain.RecognizeAsync(RecognizeMode.Single);
            Progress_Bar.Value = speechRecMain.AudioLevel;

            

        }

        //Speech to text engine
        void speechRecMain_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            rchTxtBox.AppendText(e.Result.Text);
            switch (e.Result.Text)
            {
                case "compose new mail":
                    NewMail newMailWindow = new NewMail();
                    newMailWindow.Show();
                    break;
                case "open recieved mails":
                    Main.Content = new Recieved();
                    break;
                case "open starred mails":
                    Main.Content = new Starred();
                    break;
                case "open deleted mails":
                    Main.Content = new Deleted();
                    break;
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            speechRecMain.RecognizeAsyncStop();
            Record_Button.IsEnabled = true;
            Stop_Button.IsEnabled = false;
            Progress_Bar.IsEnabled = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewMail newMailWindow = new NewMail();
            newMailWindow.Show();
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void RchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Recieved_Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Recieved();
        }

        private void Starred_Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Starred();
        }

        private void Deleted_Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Deleted();
        }

        private void Sent_Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Sent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}