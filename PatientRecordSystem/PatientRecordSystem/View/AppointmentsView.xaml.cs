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
        private enum View
        {
            Patient,
            Doctor
        }

        private View currentView = View.Patient;

        public AppointmentsView()
        {
            InitializeComponent();
            UpdatePage();
        }

        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {

        }

        private void ToggleView_Click (Object sender, RoutedEventArgs e)
        {
            SwitchView();
        }

        private void SwitchView ()
        {
            if (currentView == View.Patient)
            {
                currentView = View.Doctor;
            } 
            else
            {
                currentView = View.Patient;
            }

            UpdatePage();
        }

        private void UpdatePage ()
        {
            if (currentView == View.Patient)
            {
                ContentFrame.Navigate(new PatientAppointmentView());
                Title.Text = "Appointment Manager - Patients";
            } else if (currentView == View.Doctor)
            {
                ContentFrame.Navigate (new DoctorAppointmentView());
                Title.Text = "Appointment Manager - Doctors";
            }
        }
    }
}
