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
    /// Interaction logic for AppointmentsView.xaml
    /// </summary>
    public partial class AppointmentsView : Page
    {


        public AppointmentsView()
        {
            InitializeComponent();
            UpdatePage();
        }

        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(editing: false);
            appointmentCreationModal.ShowDialog();

            if (appointmentCreationModal.DialogResult == true)
            {
                NavigationService.Navigate(new AppointmentsView());
            }
        }



        private void UpdatePage ()
        {
            ContentFrame.Navigate (new DoctorAppointmentView());
            Title.Text = "Appointment Manager";
        }
    }
}
