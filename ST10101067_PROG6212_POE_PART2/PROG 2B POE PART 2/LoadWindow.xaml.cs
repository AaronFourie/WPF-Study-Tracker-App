using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        public LoadWindow()
        {
            InitializeComponent();
        }
        private int counter = 80;
        DateTime dt = new DateTime();
        int waitingTime = 0;

        private void LoadWindow_Load(object sender, EventArgs e)
        {
            Timer timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Start();
            Bar.Maximum = 100;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Bar.Value < 100)
            {
                Bar.Value++;
            }
            else
            {
                if (waitingTime++ > 35)
                Close();
            }
        
            

        }
    }
}
