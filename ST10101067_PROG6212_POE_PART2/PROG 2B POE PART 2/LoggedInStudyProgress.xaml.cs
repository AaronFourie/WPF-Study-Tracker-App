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

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for LoggedInStudyProgress.xaml
    /// </summary>
    public partial class LoggedInStudyProgress : Window
    {
        public LoggedInStudyProgress()
        {
            InitializeComponent();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); //Exits application
        }


        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Track_Work_Progess track = new Track_Work_Progess();
            track.DataContext = MainWindow.modules;
            track.Show();
            Close();
        }

    }
}
