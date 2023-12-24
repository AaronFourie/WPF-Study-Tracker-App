using ModuleLibrary1;
using ModuleLibrary1.ViewModel;
using PROG_2B_POE_PART_1.Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
using MessageBox = System.Windows.Forms.MessageBox;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    
    public partial class Login : Window
    {
        public static ObservableCollection<Data1> data1 = new ObservableCollection<Data1>();
        public static ObservableCollection<StudyProgress> study = new ObservableCollection<StudyProgress>();
        public ThreadFunctions thread = new ThreadFunctions();

        public Login()
        {
            InitializeComponent();
        }

        private void GetErrors()
        {

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register r = new Register();
            r.Show();
            this.Close();
        }

        private async void Continue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(LoginUsername.Text))
                {
                    string title = "Invalid Username";
                    string prompt = "Username cannot be left blank";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                }
                if (string.IsNullOrEmpty(LoginPassword.Password))
                {
                    string title = "Invalid password";
                    string prompt = "Password cannot null or empty";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                }

                else if (LoginUsername.Text.Length > 0 && LoginPassword.Password.Length > 0)
                {
                    
                    DBConnection db = new DBConnection();
                    db.TestConnection();
                    String hashed = Hashing.ToSHA256(LoginPassword.Password);
                    string check_data = "SELECT COUNT(*) FROM [dbo].[STUDENT] WHERE STUDENT_USERNAME = '" + LoginUsername.Text + "' AND STUDENT_PASSWORD = '" + hashed + "'";
                    SqlCommand cmd2 = new SqlCommand(check_data, DBConnection.con);
                    cmd2.ExecuteNonQuery();
                    int count = Convert.ToInt32(cmd2.ExecuteScalar());

                    Hide();
                    LoadWindow load = new LoadWindow();
                    load.heading.Content = "Logging in";
                    load.Show();
                    // start the job and the timer, which polls the thread
                    await Task.Run(() => thread.RetrievingData());
                    load.Close();

                    if (count == 0)
                    {
                        Show();
                        string title = "Incorrect username or password";
                        string prompt = "Try again or create a new account";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);

                    }
                    if (count == 1)
                    {

                        string title = "Login Success!";
                        string prompt = "You are logged in as " + LoginUsername.Text;
                        DialogResult d = MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (d == System.Windows.Forms.DialogResult.OK)
                        {

                            SqlDataReader dr = new SqlCommand("SELECT DISTINCT MODULE.MODULE_CODE, MODULE.MODULE_NAME, MODULE.MODULE_CREDITS, MODULE.CLASS_HOURS_PER_WEEK, STUDY.SELF_STUDY_HOURS_PER_WEEK " +
                                "FROM MODULE AS MODULE LEFT JOIN STUDY AS STUDY ON MODULE.MODULE_CODE = STUDY.MODULE_CODE WHERE MODULE.STUDENT_USERNAME = '" + LoginUsername.Text + "' ORDER BY MODULE.MODULE_CODE", DBConnection.con).ExecuteReader();

                            Hide();
                            LoadWindow retieveData = new LoadWindow();
                            retieveData.heading.Content = "Loading your data";
                            retieveData.Show();
                            // start the job and the timer, which polls the thread
                            await Task.Run(() => thread.RetrievingData());


                            while (dr.Read())
                            {
                                data1.Add(new Data1
                                {
                                    
                                    //Sets Modules Class variables to user input
                                    Code = dr.GetString(0),
                                    Name = dr.GetString(1),
                                    Credits = (double)dr.GetDecimal(2),
                                    Hours = (double)dr.GetDecimal(3),     
                                    StudyHoursPerWeek = (double)dr.GetDecimal(4),
                                });
                                
                            }
                            dr.Close();
                            DBConnection.con.Close();
                            retieveData.Close();



                            LoggedInViewModules s = new LoggedInViewModules();
                            s.DataContext = data1;
                            LoggedInViewModules.USERNAME = LoginUsername.Text;


                            dr.Close();
                            DBConnection.con.Close();
                            s.Show();
                            this.Close();

                        }

                    }
                    

                }
            }
            catch (Exception ex)
            {
                string title = "Error";
                string prompt = ex.Message.ToString();
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
            }
        }
        public class Data1
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public double Credits { get; set; }
            public double Hours { get; set; }
            public double StudyHoursPerWeek { get; set; }
        }

        private void Homebtn_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            Close();
        }
    }
}
