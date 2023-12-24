using ModuleLibrary1;
using PROG_2B_POE_PART_1.Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// Interaction logic for LoggedInViewModules.xaml
    /// </summary>
    public partial class LoggedInViewModules : Window
    {
        public static ObservableCollection<Data> study = new ObservableCollection<Data>();
        public static String USERNAME, msg;
        public LoggedInViewModules()
        {
            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            DBConnection a = new DBConnection();
            a.TestConnection();

            SqlDataReader dr2 = new SqlCommand("SELECT SUM(HOURS_STUDIED) AS HOURS_STUDIED, CURRENT_WEEK, MODULE_CODE , MIN(STUDY_HOURS_REMAINDING) AS STUDYS, " +
                "SUM(HOURS_STUDIED) * 100/(SELECT MIN(SELF_STUDY_HOURS_PER_WEEK) FROM STUDY) AS 'COMPLETION'" +
                "FROM STUDY WHERE STUDENT_USERNAME = '" + USERNAME + "' GROUP BY MODULE_CODE, CURRENT_WEEK" , DBConnection.con).ExecuteReader();
            
            while (dr2.Read())
            {
                double status = (double)dr2.GetDecimal(4);
                switch(status)
                {
                    case 0:
                        msg = "Haven't started yet";
                        break;
                    case double i when i >0 && i <=25:
                        msg = "Barely started";
                        break;
                    case double i when i > 25 && i < 50:
                        msg = "Almost halfway";
                        break;
                    case 50:
                        msg = "Halfway done";
                        break;
                    case double i when i > 50 && i <= 75:
                        msg = "Slightly more than halway done";
                        break;
                    case double i when i > 75 && i < 100:
                        msg = "Almost done";
                        break;
                    case double i when i >= 100:
                        msg = "Done";
                        break;

                }
                study.Add(new Data
                {
                    //Sets Modules Class variables to user input
                    Code = dr2.GetString(2),
                    Hours = (double)dr2.GetDecimal(0),
                    Week = dr2.GetInt32(1),
                    StudyHoursRemainding = (double)dr2.GetDecimal(3),
                    WeekStudyCompletion = (String)((double)dr2.GetDecimal(4) + " %"),
                    Status = msg,
                    
                });;
            }

           
           

            LoggedInStudyProgress p = new LoggedInStudyProgress();
            p.DataContext = study; //setting datacontext of list of modules with the study hours remainding here since i didnt know how to set it otherwise
            p.Show();
            dr2.Close();
            DBConnection.con.Close();   
            this.Close();

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); //Exits application
        }

        public class Data
        {
            public string Code { set; get; }
            public double SelfStudyHours { set; get; }
            public double Hours { get; set; }
            public double StudyHoursRemainding { set; get; }
            public int Week { set; get; }
            public string WeekStudyCompletion { set; get; }
            public string Status { set; get; }
        }

    }
    
}
