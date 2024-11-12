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

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {

        Page TargetPage;

        public NotificationWindow(string header, string message, Page targetPage)
        {

            InitializeComponent();

            HeaderText.Text = header;
            ContentText.Text = message;
            TargetPage = targetPage;
        }

        private void Close_Click (object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(TargetPage);
            this.Close();
        }
    }
}
