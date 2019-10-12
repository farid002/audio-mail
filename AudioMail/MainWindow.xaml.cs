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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;


namespace AudioMail
{
    
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine speechRecMain = new SpeechRecognitionEngine();

        public List<Mail> SentMailList { get; set; }
        public List<Mail> StarredMailList { get; set; }
        public List<Mail> ReceivedMailList { get; set; }
        public List<Mail> DeletedMailList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "create new mail", "open sent mails", "open recieved mails", "open deleted mails", "open starred mails" });
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);
            Grammar grammar = new Grammar(grammarBuilder);
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
            speechRecMain.RecognizeAsync(RecognizeMode.Multiple);
            Progress_Bar.Value = speechRecMain.AudioLevel;
        }

        //Speech to text engine
        void speechRecMain_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "create new mail":
                    ButtonAutomationPeer peer = new ButtonAutomationPeer(NewMail_Button);
                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                    break;
                case "open sent mails":
                    break;
                case "open recieved mails":
                    break;
                case "open starred mails":
                    break;
                case "open deleted mails":
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


        private void Recieved_Button_Click(object sender, RoutedEventArgs e)
        {
            SentList.Visibility = Visibility.Hidden;
            DeletedList.Visibility = Visibility.Hidden;
            StarredList.Visibility = Visibility.Hidden;
            ReceivedList.Visibility = Visibility.Visible;
        }

        private void Starred_Button_Click(object sender, RoutedEventArgs e)
        {
            SentList.Visibility = Visibility.Hidden;
            DeletedList.Visibility = Visibility.Hidden;
            StarredList.Visibility = Visibility.Visible;
            ReceivedList.Visibility = Visibility.Hidden;
        }

        private void Deleted_Button_Click(object sender, RoutedEventArgs e)
        {
            SentList.Visibility = Visibility.Hidden;
            DeletedList.Visibility = Visibility.Visible;
            StarredList.Visibility = Visibility.Hidden;
            ReceivedList.Visibility = Visibility.Hidden;
        }

        private void Sent_Button_Click(object sender, RoutedEventArgs e)
        {
            DeletedList.Visibility = Visibility.Hidden;
            StarredList.Visibility = Visibility.Hidden;
            ReceivedList.Visibility = Visibility.Hidden;
            SentList.Visibility = Visibility.Visible;
        }

        private void SentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StarredList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeletedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ReceivedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewMail_Button_Click(object sender, RoutedEventArgs e)
        {
            NewMail_GroupBox.Visibility = Visibility.Visible;
        }


        private void Send_Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(To_TextBox.Text))
            {
                //NewMail_GroupBox.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please enter email address!");
            }
            NewMail_RichTextBox.AppendText(To_TextBox.Text);
        }
    }
}