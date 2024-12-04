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

         /// <summary>
         /// Accepts optional parameters 'visible' and 'date'
         /// </summary>
         /// <param name="visible">Determines whether the schedule panel is visible or not</param>
         /// <param name="date">Determins the default date to show when opening the schedule panel</param>
        public DoctorAppointmentView(bool visible = false, DateTime date = new DateTime ())
        {
            InitializeComponent();

            // If the logged in user is of type doctor, only show the current user in the DataGrid
            if (UserManager.GetInstance ().currentUser.AccountType == User.UserAccountType.Doctor)
            {
                Doctors = new List<User>();
                Doctors.Add(UserManager.GetInstance().currentUser);
            }
            DoctorTable.DataContext = Doctors;
            currentDoctor = new User();

            // If visible is false, set the default date to today.
            if (!visible)
            {
                SBDatePicker.SelectedDate = DateTime.Today;
            } else
            {
                SBDatePicker.SelectedDate = date;
            }
            
            Visible = visible;

            // Set the current doctor in the Globals class
            currentDoctor = Globals.appointmentViewDoctor;
            Globals.appointmentViewDoctor = new User();

            UpdateSchedule();
        }

        /// <summary>
        /// Opens the schedule view
        /// </summary>

        private void Schedule_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = DoctorTable.SelectedItem as User;
            Visible = true;
            UpdateSchedule();
        }

        /// <summary>
        /// Closes the schedule view
        /// </summary>
        private void Close_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = new User();
            Visible = false;
            UpdateSchedule();
        }
        
        /// <summary>
        /// Opens the AppointmentCreationModal window with the selected appointment passed in as parameters
        /// </summary>
        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {

            if (UserManager.GetInstance().currentUser.AccountType == User.UserAccountType.Admin)
            {
                return;
            }
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(date: DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate), time: (SBSchedule.SelectedItem as Appointment).Time, slot: SBSchedule.SelectedIndex, doctor: currentDoctor.Username, editing: true);
            Globals.appointmentViewDoctor = currentDoctor;
            appointmentCreationModal.ShowDialog();

            // When the window is closed, update the schedule for the doctor, and refresh the page.
            if (appointmentCreationModal.DialogResult == true)
            {
                UpdateSchedule();
                NavigationService.Navigate(new DoctorAppointmentView(true, (DateTime)SBDatePicker.SelectedDate));
            }
        }

        /// <summary>
        /// If the currently logged in user is of type doctor, open a readonly AppointmentCreationModal with the doctor parameter set as true. otherwise, se the doctor parameter to false.
        /// </summary>
        private void AppointmentDetails_Click (object sender, RoutedEventArgs e)
        {
            if (UserManager.GetInstance ().currentUser.AccountType == User.UserAccountType.Doctor)
            {
               AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(true, DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate), (SBSchedule.SelectedItem as Appointment).Time, SBSchedule.SelectedIndex, currentDoctor.Username, (SBSchedule.SelectedItem as Appointment).PatientId, (SBSchedule.SelectedItem as Appointment).BriefDescription, (SBSchedule.SelectedItem as Appointment).Description, true);

                Globals.appointmentViewDoctor = currentDoctor;
                appointmentCreationModal.ShowDialog();

                if (appointmentCreationModal.DialogResult == true)
                {
                    NavigationService.Navigate(new DoctorAppointmentView(true, (DateTime)SBDatePicker.SelectedDate));
                }
            } else
            {
                AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(true, DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate), (SBSchedule.SelectedItem as Appointment).Time, SBSchedule.SelectedIndex, currentDoctor.Username, (SBSchedule.SelectedItem as Appointment).PatientId, (SBSchedule.SelectedItem as Appointment).BriefDescription, (SBSchedule.SelectedItem as Appointment).Description, editing: false);

                Globals.appointmentViewDoctor = currentDoctor;
                appointmentCreationModal.ShowDialog();

                if (appointmentCreationModal.DialogResult == true)
                {
                    NavigationService.Navigate(new DoctorAppointmentView(true, (DateTime)SBDatePicker.SelectedDate));
                }
            }
        }

        /// <summary>
        /// Update the schedule upon date change
        /// </summary>
        private void Date_Changed (object sender, RoutedEventArgs e)
        {
            UpdateSchedule();
        }

        /// <summary>
        /// Updates the schedule dependant on the current doctor, and current selected date.
        /// </summary>
        private void UpdateSchedule ()
        {
            // If visible is true
            if (Visible)
            {
                // If the date hasn't been assigned, set it to today's date.
                if (SBDatePicker.SelectedDate == null)
                {
                    SBDatePicker.SelectedDate = DateTime.Today;
                }
                
                // Update the grid spans and set the schedule box's visibility to true.
                Grid.SetColumnSpan(DoctorTable, 1);
                Grid.SetColumnSpan(ScheduleBox, 1);
                ScheduleBox.Visibility = Visibility.Visible;
                
                SBTitle.Text = $"Schedule for {currentDoctor.FirstName} {currentDoctor.LastName}";

                SBSchedule.DataContext = currentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate));

                // If the date has been assigned, get the appointments for the current doctor for that date
                if (SBDatePicker.SelectedDate != null) { 

                    SBSchedule.DataContext = currentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate));
                }
            } 
            else
            {
                // If visible is false, hide the schedule box
                ScheduleBox.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(DoctorTable, 2);
                Grid.SetColumnSpan(ScheduleBox, 1);
            }
        }
    }
}
