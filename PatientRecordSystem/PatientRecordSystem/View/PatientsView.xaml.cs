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
using PatientRecordSystem.Util;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Page
    {
        public PatientsView()
        {
            InitializeComponent();
        }

        private void Test_Click (object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(PatientManager.GetInstance().Patients()[0].Address.ParsedAddress());
            Trace.WriteLine(PatientManager.GetInstance().Patients()[0].HospitalNumber());
        }
    }
}
