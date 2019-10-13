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
using System.Speech.Synthesis;


namespace AudioMail
{
    
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine speechRecMain = new SpeechRecognitionEngine();
        SpeechRecognitionEngine speechRecNewMail = new SpeechRecognitionEngine();
        SpeechSynthesizer synthNewMail = new SpeechSynthesizer();

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
            //Declaration and definition of command voice engine
            Choices commands = new Choices();
            commands.Add(new string[] { "create new mail", "open sent mails", "open recieved mails", "open deleted mails", "open starred mails" });
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecMain.LoadGrammarAsync(grammar);
            speechRecMain.SetInputToDefaultAudioDevice();
            speechRecMain.SpeechRecognized += speechRecMain_SpeechRecognized;


            //Declaration and definition of new mail speech-to-text engine
            DictationGrammar dictationGrammar = new DictationGrammar();
            speechRecNewMail.LoadGrammarAsync(dictationGrammar);
            speechRecNewMail.SetInputToDefaultAudioDevice();
            speechRecNewMail.SpeechRecognized += speechRecNewMail_SpeechRecognized;

            //Declaration and definition of new mail text-to-speech engine
            
            synthNewMail.SetOutputToDefaultAudioDevice();
            StopNewMail_Button.IsEnabled = false;
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
                    ButtonAutomationPeer peerNewMail = new ButtonAutomationPeer(NewMail_Button);
                    IInvokeProvider invokeProvNewMail = peerNewMail.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvNewMail.Invoke();
                    break;
                case "open sent mails":
                    ButtonAutomationPeer peerSent = new ButtonAutomationPeer(Sent_Button);
                    IInvokeProvider invokeProvSent = peerSent.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvSent.Invoke();
                    break;
                case "open recieved mails":
                    ButtonAutomationPeer peerReceived = new ButtonAutomationPeer(Received_Button);
                    IInvokeProvider invokeProvReceived = peerReceived.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvReceived.Invoke();
                    break;
                case "open starred mails":
                    ButtonAutomationPeer peerStarred = new ButtonAutomationPeer(Starred_Button);
                    IInvokeProvider invokeProvStarred = peerStarred.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvStarred.Invoke();
                    break;
                case "open deleted mails":
                    ButtonAutomationPeer peerDeleted = new ButtonAutomationPeer(Deleted_Button);
                    IInvokeProvider invokeProvDeleted = peerDeleted.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvDeleted.Invoke();
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
            ListLabel.Content = "RECEIVED MAILS";
        }

        private void Starred_Button_Click(object sender, RoutedEventArgs e)
        {
            SentList.Visibility = Visibility.Hidden;
            DeletedList.Visibility = Visibility.Hidden;
            StarredList.Visibility = Visibility.Visible;
            ReceivedList.Visibility = Visibility.Hidden;
            ListLabel.Content = "STARRED MAILS";
        }

        private void Deleted_Button_Click(object sender, RoutedEventArgs e)
        {
            SentList.Visibility = Visibility.Hidden;
            DeletedList.Visibility = Visibility.Visible;
            StarredList.Visibility = Visibility.Hidden;
            ReceivedList.Visibility = Visibility.Hidden;
            ListLabel.Content = "DELETED MAILS";
        }

        private void Sent_Button_Click(object sender, RoutedEventArgs e)
        {
            DeletedList.Visibility = Visibility.Hidden;
            StarredList.Visibility = Visibility.Hidden;
            ReceivedList.Visibility = Visibility.Hidden;
            SentList.Visibility = Visibility.Visible;
            ListLabel.Content = "SENT MAILS";
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
            To_TextBox.Text = "";
            Subject_TextBox.Text = "";
            NewMail_RichTextBox.SelectAll();
            NewMail_RichTextBox.Selection.Text = "";
            NewMail_GroupBox.Visibility = Visibility.Visible;
        }


        private void Send_Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (!String.IsNullOrEmpty(To_TextBox.Text))
            {
                //Create new "Mail" object and store it in proper Lists
                
                Mail newmail = new Mail();
                string myText;
                NewMail_RichTextBox.SelectAll();
                if(NewMail_RichTextBox.Selection.Text.Length > 30){
                    myText = new TextRange(NewMail_RichTextBox.Document.ContentStart, NewMail_RichTextBox.Document.ContentStart.GetPositionAtOffset(30)).Text;
                }
                else
                {
                    myText = new TextRange(NewMail_RichTextBox.Document.ContentStart, NewMail_RichTextBox.Document.ContentEnd).Text;
                }
                
                newmail.Title = Subject_TextBox.Text + ":   " + myText + "...";
                newmail.Date = DateTime.UtcNow.ToString();
                NewMail_RichTextBox.SelectAll();
                newmail.Content = NewMail_RichTextBox.Selection.Text;

                //Adding to Sentlist
                SentList.Items.Add(newmail);
                //Adding to ReceivedList
                ReceivedList.Items.Add(newmail);
                NewMail_GroupBox.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please enter email address!");
            }
            NewMail_RichTextBox.AppendText(To_TextBox.Text);
        }

        private void RecordNewMail_Button_Click(object sender, RoutedEventArgs e)
        {
            RecordNewMail_Button.IsEnabled = false;
            StopNewMail_Button.IsEnabled = true;
            speechRecNewMail.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void StopNewMail_Button_Click(object sender, RoutedEventArgs e)
        {
            speechRecNewMail.RecognizeAsyncStop();
            RecordNewMail_Button.IsEnabled = true;
            StopNewMail_Button.IsEnabled = false;
        }

        void speechRecNewMail_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            NewMail_RichTextBox.AppendText(e.Result.Text);
        }

        private void ListenNewMail_Button_Click(object sender, RoutedEventArgs e)
        {
            // Speak a string.
            NewMail_RichTextBox.SelectAll();
            synthNewMail.Speak(NewMail_RichTextBox.Selection.Text);
        }

        private void CloseNewMail_Button_Click(object sender, RoutedEventArgs e)
        {
            NewMail_GroupBox.Visibility = Visibility.Hidden;

            //Delete new mail contents
            To_TextBox.Text = "";
            Subject_TextBox.Text = "";
            NewMail_RichTextBox.SelectAll();
            NewMail_RichTextBox.Selection.Text = "";
            
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (SentList.SelectedItem != null)
            {
                Mail mail = (Mail)SentList.SelectedItem;
                DeletedList.Items.Add(mail);
                SentList.Items.RemoveAt(SentList.Items.IndexOf(SentList.SelectedItem));
                System.Windows.MessageBox.Show("Deleted!");
                CurrentMail_GroupBox.Visibility = Visibility.Hidden;
            }
            else if (ReceivedList.SelectedItem != null)
            {
                Mail mail = (Mail)ReceivedList.SelectedItem;
                DeletedList.Items.Add(mail);
                ReceivedList.Items.RemoveAt(ReceivedList.Items.IndexOf(ReceivedList.SelectedItem));
                System.Windows.MessageBox.Show("Deleted!");
                CurrentMail_GroupBox.Visibility = Visibility.Hidden;
            }
            else if (StarredList.SelectedItem != null)
            {
                Mail mail = (Mail)StarredList.SelectedItem;
                DeletedList.Items.Add(mail);
                SentList.Items.RemoveAt(StarredList.Items.IndexOf(StarredList.SelectedItem));
                System.Windows.MessageBox.Show("Deleted!");
                CurrentMail_GroupBox.Visibility = Visibility.Hidden;
            }
            else if (DeletedList.SelectedItem != null)
            {
                Mail mail = (Mail)DeletedList.SelectedItem;
                DeletedList.Items.RemoveAt(DeletedList.Items.IndexOf(DeletedList.SelectedItem));
                System.Windows.MessageBox.Show("Permanently Deleted!");
                CurrentMail_GroupBox.Visibility = Visibility.Hidden;
            }
        }

        private void MakeStarred_Button_Click(object sender, RoutedEventArgs e)
        {
            if (SentList.SelectedItem != null)
            {
                Mail mail = (Mail)SentList.SelectedItem;
                StarredList.Items.Add(mail);
                System.Windows.MessageBox.Show("Starred!");
            }
            else if (ReceivedList.SelectedItem != null)
            {
                Mail mail = (Mail)ReceivedList.SelectedItem;
                StarredList.Items.Add(mail);
                System.Windows.MessageBox.Show("Starred!");
            }
            else if (StarredList.SelectedItem != null)
            {
                Mail mail = (Mail)StarredList.SelectedItem;
                StarredList.Items.Add(mail);
                System.Windows.MessageBox.Show("Starred!");
            }
            else if (DeletedList.SelectedItem != null)
            {
                Mail mail = (Mail)DeletedList.SelectedItem;
                StarredList.Items.Add(mail);
                System.Windows.MessageBox.Show("Starred!");
            }
        }

        private void CloseCurrentMail_Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentMail_GroupBox.Visibility = Visibility.Hidden;
        }

        private void SentList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Mail currentmail = (Mail)SentList.SelectedItem;
            CurrentMail_GroupBox.Visibility = Visibility.Visible;
            CurrentMail_RichTextBox.AppendText(currentmail.Content);
        }

        private void ReceivedList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentMail_GroupBox.Visibility = Visibility.Visible;
        }

        private void DeletedList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentMail_GroupBox.Visibility = Visibility.Visible;
        }

        private void StarredList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentMail_GroupBox.Visibility = Visibility.Visible;
        }
    }
}