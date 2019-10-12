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



namespace AudioMail
{
    /// <summary>
    /// Interaction logic for Starred.xaml
    /// </summary>
    public partial class Starred : Page
    {
        public List<Mail> StarredMailList { get; set; }

        public Starred()
        {
            InitializeComponent();
            StarredMailList = new List<Mail>();
            Mail mail1 = new Mail();
            mail1.Title = "sdsds";
            DateTime date = DateTime.UtcNow.Date;
            mail1.Date = date.ToString("dd/MM/yyyy");

            StarredMailList.Add(mail1);
            DataContext = this;

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

 
}
