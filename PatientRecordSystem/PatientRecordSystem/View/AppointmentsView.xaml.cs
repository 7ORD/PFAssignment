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
    /// Interaction logic for AppointmentsView.xaml
    /// </summary>
    public partial class AppointmentsView : Page
    {
        public AppointmentsView()
        {
            InitializeComponent();

            if (UserManager.GetInstance ().currentUser.AccountType == Model.User.UserAccountType.Admin)
            {
                NewAppointmentButton.IsEnabled = false;
            }

            UpdatePage();
        }

        /// <summary>
        /// Opens an AppointmentCreationModal dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(editing: true);
            appointmentCreationModal.ShowDialog();

            // Refresh the page when the dialog is closed.
            if (appointmentCreationModal.DialogResult == true)
            {
                NavigationService.Navigate(new AppointmentsView());
            }
        }


        /// <summary>
        /// Updates the page 
        /// </summary>
        private void UpdatePage ()
        {
            ContentFrame.Navigate (new DoctorAppointmentView());
            Title.Text = "Appointment Manager";
        }
    }
}
