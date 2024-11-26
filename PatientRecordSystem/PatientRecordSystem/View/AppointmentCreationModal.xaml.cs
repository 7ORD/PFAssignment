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

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for AppointmentCreationModal.xaml
    /// </summary>
    public partial class AppointmentCreationModal : Window
    {
        public AppointmentCreationModal()
        {
            InitializeComponent();
        }

        private void DateSelector_Changed (object sender, RoutedEventArgs e)
        {

        }

        private void Submit_Click (object sender, RoutedEventArgs e)
        {
            
        }

        private void Cancel_Click (object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
