using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for AppointmentCreationModal.xaml
    /// </summary>
    public partial class AppointmentCreationModal : Window
    {
        private Appointment newAppointment = new Appointment();
        private string patientId;
        private bool ReadOnly = false;
        /// <summary>
        /// Constructor for the new appointment window
        /// </summary>
        /// <param name="readOnly">Is the form readonly?</param>
        /// <param name="date">Prepopulated date</param>
        /// <param name="time">Prepopulated time</param>
        /// <param name="slot">Prepopulated slot</param>
        /// <param name="doctor">Prepopulated doctor</param>
        /// <param name="patient">Prepopulated patient</param>
        /// <param name="summary">Prepopulated summary</param>
        /// <param name="description">Prepopulated description</param>
        /// <param name="editing">Is the form being edited?</param>
        public AppointmentCreationModal(bool readOnly = false, DateOnly date = new DateOnly(), TimeOnly time = new TimeOnly(), int slot = -1, string doctor = "", string patient = "", string summary = "", string description = "", bool editing = false)
        {
            Trace.WriteLine(editing);

            InitializeComponent();

            ReadOnly = readOnly;
            patientId = patient;

            // The following if statements will prepopulate the form dependant on whether data has been passed into the constructor.
            if (date != new DateOnly () && time != new TimeOnly () && slot > -1)
            {
                AppointmentTime.Text = $"{date} - {time}";
                newAppointment.Date = date;
                newAppointment.Time = time;
                newAppointment.Slot = slot;
            }

            if (!string.IsNullOrEmpty (doctor))
            {
                Doctor.Text = UserManager.GetInstance().Users().Find(u => u.Username == doctor).ParsedName;
                newAppointment.Doctor = doctor;
            }

            if (!string.IsNullOrEmpty (patient))
            {
                Patient.Text = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == patient).ParsedName;
                newAppointment.PatientId = patient;
            }

            if (!string.IsNullOrEmpty (summary))
            {
                Summary.Text = summary;
                newAppointment.BriefDescription = summary;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description.Text = description;
                newAppointment.Description = description;
            }

            if (readOnly)
            {
                Description.IsEnabled = false;
                Summary.IsEnabled = false;
                SelectDateButton.IsEnabled = false;
                SelectDoctorButton.IsEnabled = false;
                SelectPatientButton.IsEnabled = false;
                ReadOnly = readOnly;
                DoctorNotes.IsEnabled = false;
                AppointmentStatus.IsEnabled = false;

                Submit.Visibility = Visibility.Hidden;
                Grid.SetColumn(Cancel, 0);
                Grid.SetColumnSpan(Cancel, 2);
                Title.Text = $"Appointment for {Patient.Text = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == patient).ParsedName}";
            }

            // Checks if the form is editing, and if so, let the user edit the AppointmentStatus and DoctorNotes
            if (!editing)
            {
                AppointmentStatus.IsEnabled = false;


                SelectDoctorButton.IsEnabled = false;
                SelectPatientButton.IsEnabled = false;
            } else
            {
                AppointmentStatus.IsEnabled = true;
                DoctorNotes.IsEnabled = true;
                ReadOnly = false;
                if (UserManager.GetInstance().currentUser.AccountType != User.UserAccountType.Doctor)
                {
                    SelectDoctorButton.IsEnabled = true;
                    SelectPatientButton.IsEnabled = true;
                }
            }

            // If the user isn't of type Doctor - Don't let them edit the doctor's notes
            if (UserManager.GetInstance().currentUser.AccountType != User.UserAccountType.Doctor)
            {
                DoctorNotes.IsEnabled = false;
            }

            // If the user is of type Admin, hide the doctor's notes and appointment description, otherwise, show it.
            if (UserManager.GetInstance ().currentUser.AccountType == User.UserAccountType.Admin)
            {

                DescriptionLabel.Visibility = Visibility.Collapsed;
                Description.Visibility = Visibility.Collapsed;
                DoctorNotesLabel.Visibility = Visibility.Collapsed;
                DoctorNotes.Visibility = Visibility.Collapsed;
            } else
            {
                DescriptionLabel.Visibility = Visibility.Visible;
                Description.Visibility = Visibility.Visible;
                DoctorNotesLabel.Visibility = Visibility.Visible;
                DoctorNotes.Visibility = Visibility.Visible;
            }

            AppointmentStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Opens a new dialog window of type DoctorSelectionModal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDoctor_Click (object sender, RoutedEventArgs e)
        {
            DoctorSelectionModal doctorSelectionModal = new DoctorSelectionModal();
            doctorSelectionModal.ShowDialog();

            // When window closes, populate the form with the selected doctor.
            if (doctorSelectionModal.DialogResult == true)
            {
                Doctor.Text = Globals.newAppointmentDoctor.ParsedName;
                newAppointment.Doctor = Globals.newAppointmentDoctor.Username;
                if (newAppointment.Doctor != null)
                {
                    SelectDateButton.IsEnabled = true;
                }

                Validate(Doctor, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Opens a new dialog window of type PatientSelectionModal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPatient_Click(object sender, RoutedEventArgs e)
        {
            PatientSelectionModal patientSelectionModal = new PatientSelectionModal();
            patientSelectionModal.ShowDialog();

            // When window closes, populate the form with the selected patient.
            if (patientSelectionModal.DialogResult == true)
            {
                Patient.Text = Globals.newAppointmentPatient.ParsedName;
                newAppointment.PatientId = Globals.newAppointmentPatient.HospitalNumber;
                Validate(Patient, new RoutedEventArgs());
            }
        }

        // Opens a new dialog window of type SlotSelectionModal
        private void SelectSlot_Click (object sender, RoutedEventArgs e)
        {
            SlotSelectionModal slotSelectionModal = new SlotSelectionModal();
            slotSelectionModal.ShowDialog();

            // When window closes, populate the form with the selected timeslot.
            if (slotSelectionModal.DialogResult == true)
            {
                AppointmentTime.Text = $"{Globals.newAppointmentDate} - {Globals.newAppointmentTime}";
                newAppointment.Date = Globals.newAppointmentDate;
                newAppointment.Slot = Globals.newAppointmentSlot;

                Validate(SelectDateButton, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Validates data entered into the form with the AppointmentManager.IsAppointmentValid method.
        /// </summary>
        private void Validate (object sender, RoutedEventArgs e)
        {

            UpdateAppointmentDetails();

            if (AppointmentManager.GetInstance ().IsAppointmentValid (newAppointment))
            {
                Submit.IsEnabled = true;
            } else
            {
            if (Submit != null)
            {
                    Submit.IsEnabled = false;
                }   
            }
        }

        /// <summary>
        /// Closes the window
        /// </summary>

        private void Cancel_Click (object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Creates a new appointment, saves it into the appointments.json file, and adds the appointment ID to the doctor's user file in users.json
        /// </summary>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> appointments = AppointmentManager.GetInstance().Appointments();
            List<User> users = UserManager.GetInstance().Users();

            appointments.Add(newAppointment);
            users.Find(u => u.Username == newAppointment.Doctor).Appointments.Add(newAppointment.AppointmentId);

            AppointmentManager.GetInstance().UpdateData(appointments);
            UserManager.GetInstance().UpdateData(users);

            NotificationWindow notification = new NotificationWindow("Appointment Scheduled", "A new appointment has been entered in the system.");
            notification.ShowDialog();

            Globals.appointmentViewDoctor = users.Find(u => u.Username == newAppointment.Doctor);

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Updates the newAppointment property with the form's data
        /// </summary>
        private void UpdateAppointmentDetails ()
        {
            newAppointment.AppointmentId = AppointmentManager.GetInstance().Appointments().Count;

            if (Summary != null)
            {
                newAppointment.BriefDescription = Summary.Text;
            }
            
            if (Description != null)
            {
                newAppointment.Description = Description.Text;
            }
            
            newAppointment.AppointmentCreator = UserManager.GetInstance().currentUser.Username;
        }
    }
}
