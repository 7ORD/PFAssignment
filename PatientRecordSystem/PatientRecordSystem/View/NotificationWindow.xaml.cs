using PatientRecordSystem.Util;
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

        public string Header;
        public string Message;
        
        /// <summary>
        /// Sets the header and message of the notification window to strings 'header' and 'message'
        /// </summary>
        /// <param name="header">The title of the notification window</param>
        /// <param name="message">The message to display in the notification window</param>
        public NotificationWindow(string header, string message)
        {
            InitializeComponent();

            Header = header;
            Message = message;

            HeaderText.Text = Header;
            ContentText.Text = Message;
        }
        /// <summary>
        /// Closes the notification window
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
