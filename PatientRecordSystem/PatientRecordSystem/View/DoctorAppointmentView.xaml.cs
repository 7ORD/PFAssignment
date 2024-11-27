using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for DoctorAppointmentView.xaml
    /// </summary>
    public partial class DoctorAppointmentView : Page
    {

        private List<User> Doctors = UserManager.GetInstance ().Users ().Where (u => u.AccountType == User.UserAccountType.Doctor).ToList ();
        private User currentDoctor;
        private List<Appointment> appointments;
        private bool Visible;

        public DoctorAppointmentView(bool visible = false)
        {
            InitializeComponent();
            DoctorTable.DataContext = Doctors;
            currentDoctor = new User();

            SBDatePicker.SelectedDate = DateTime.Today;

            Visible = visible;

            currentDoctor = Globals.appointmentViewDoctor;
            Globals.appointmentViewDoctor = new User();

            UpdateSchedule();
        }

        private void Schedule_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = DoctorTable.SelectedItem as User;
            Visible = true;
            UpdateSchedule();
        }

        private void Close_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = new User();
            Visible = false;
            UpdateSchedule();
        }
        
        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(date: DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate), time: (SBSchedule.SelectedItem as Appointment).Time, slot: SBSchedule.SelectedIndex, doctor: currentDoctor.Username);
            Globals.appointmentViewDoctor = currentDoctor;
            appointmentCreationModal.ShowDialog();

            if (appointmentCreationModal.DialogResult == true)
            {
                UpdateSchedule();
                NavigationService.Navigate(new DoctorAppointmentView(true));
            }
        }

        private void AppointmentDetails_Click (object sender, RoutedEventArgs e)
        {
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(true, DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate), (SBSchedule.SelectedItem as Appointment).Time, SBSchedule.SelectedIndex, currentDoctor.Username, (SBSchedule.SelectedItem as Appointment).PatientId, (SBSchedule.SelectedItem as Appointment).BriefDescription, (SBSchedule.SelectedItem as Appointment).Description);
            Globals.appointmentViewDoctor = currentDoctor;
            appointmentCreationModal.ShowDialog();

            if (appointmentCreationModal.DialogResult == true)
            {
                NavigationService.Navigate(new DoctorAppointmentView(true));
            }
        }

        private void Date_Changed (object sender, RoutedEventArgs e)
        {
            UpdateSchedule();
        }

        private void UpdateSchedule ()
        {
            if (Visible)
            {
                if (SBDatePicker.SelectedDate == null)
                {
                    SBDatePicker.SelectedDate = DateTime.Today;
                }
                
                Grid.SetColumnSpan(DoctorTable, 1);
                Grid.SetColumnSpan(ScheduleBox, 1);
                ScheduleBox.Visibility = Visibility.Visible;
                
                SBTitle.Text = $"Schedule for {currentDoctor.FirstName} {currentDoctor.LastName}";

                if (SBDatePicker.SelectedDate != null) { 

                    SBSchedule.DataContext = currentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate));
                }
            } 
            else
            {
                ScheduleBox.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(DoctorTable, 2);
                Grid.SetColumnSpan(ScheduleBox, 1);
            }
        }
    }
}
