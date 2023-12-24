using ModuleLibrary1;
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
    /// Interaction logic for ModuleList.xaml
    /// </summary>
    public partial class ModuleList : Window
    {
        
        public ModuleList()
        {
            InitializeComponent();

            
            DataContext = MainWindow.modules;
            

        }

        private void ModuleFormHeading_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Continue_Click(object sender, RoutedEventArgs e)//Moves on to the next window and closes the current
        {
            Track_Work_Progess t = new Track_Work_Progess();
            t.Show();
            Close();
        }

    }
}
