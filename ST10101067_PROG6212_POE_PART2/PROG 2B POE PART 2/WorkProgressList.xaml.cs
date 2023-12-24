using System.Windows;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for WorkProgressList.xaml
    /// </summary>
    public partial class WorkProgressList : Window
    {
        public WorkProgressList()
        {
            
            InitializeComponent();
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)//Opens the prvious window and closes the current
        {
            Track_Work_Progess work = new Track_Work_Progess();
            work.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); //Exits application
        }

    }
}
