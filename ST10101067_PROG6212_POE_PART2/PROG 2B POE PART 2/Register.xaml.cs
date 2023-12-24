using ModuleLibrary1;
using ModuleLibrary1.ViewModel;
using PROG_2B_POE_PART_1.Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Timers.Timer;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public static ObservableCollection<Account> ac = new ObservableCollection<Account>();
        public ThreadFunctions thread = new ThreadFunctions();
        public static string USERNAME;
        public static Uri u;
        
        public Register()
        {
            
            InitializeComponent();
        }
        DispatcherTimer timer = new DispatcherTimer();

        private async void Registerbtn_Click(object sender, RoutedEventArgs e)
        {

            try
                {
                    if(Password1.Password != Password2.Password)
                    {
                        string title = "Incorrect Password";
                        string prompt = "Passwords do not match: \n\n";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                    }
                    if (string.IsNullOrEmpty(Username.Text))
                    {
                        string title = "Invalid Username";
                        string prompt = "Username cannot be left blank";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                    }
                    if (string.IsNullOrEmpty(Password1.Password))
                    {
                        string title = "Invalid password";
                        string prompt = "Password cannot null or empty";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                    }

                    else if(Username.Text.Length > 0 && Password1.Password.Length > 0 && (Password1.Password == Password2.Password))
                    {
                        
                        DBConnection db = new DBConnection();
                        db.TestConnection();
                       
                        string check_data = "SELECT COUNT(*) FROM [dbo].[STUDENT] WHERE STUDENT_USERNAME = '"+Username.Text+"' ";
                        SqlCommand cmd2 = new SqlCommand(check_data, DBConnection.con);
                        cmd2.ExecuteNonQuery();
                        int count = Convert.ToInt32(cmd2.ExecuteScalar());

                        Hide();
                        LoadWindow load = new LoadWindow();
                        load.heading.Content = "Checking username";
                        load.Show();
                        // start the job and the timer, which polls the thread
                        await Task.Run(() => thread.RetrievingData());
                        load.Close();

                        
                        if (count != 0)
                        {
                            Show();
                            string title = "Username Already exits";
                            string prompt = "Try another Username";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                        }
                        if(count == 0)
                        {
                        Account acd = new Account();
                        LoadWindow insert = new LoadWindow();
                        insert.Show();                     
                        load.Content = "Inserting Data";
                        await Task.Run(() => thread.Loading());

                        string add_data = "INSERT INTO [dbo].[STUDENT] VALUES(@STUDENT_USERNAME, @STUDENT_PASSWORD)";
                        SqlCommand cmd = new SqlCommand(add_data, DBConnection.con);
                            cmd.Parameters.AddWithValue("@STUDENT_USERNAME", Username.Text);
                            cmd.Parameters.AddWithValue("@STUDENT_PASSWORD", Hashing.ToSHA256(Password1.Password));
                            cmd.ExecuteNonQuery();

                        insert.Close();
                        Show();
                                                       
                           //multithreading sleep before register success
                            string title = "Success";
                            string prompt = "Data saved into database correctly";
                            DialogResult d = MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (d == System.Windows.Forms.DialogResult.OK)
                            {
                            ac.Add(new Account
                            {
                                //Sets Modules Class variables to user input
                                Username = (String)Username.Text,
                                Password = (String)Password1.Password,
                            }) ;
                                USERNAME = (String)Username.Text;

                                Semester m = new Semester();                 
                                m.Show();
                                Close();
                            }

                        }
                    DBConnection.con.Close();

                    }
                }
                catch(Exception ex)
                {
                    string title = "Error";
                    string prompt = ex.Message.ToString();
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                }
            }
        

        private void Loginbtn_Click(object sender, RoutedEventArgs e)
        {           
            Login l = new Login();
            l.Show();
            this.Close();
        }

        private void Homebtn_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            Close();
        }

      
    }
}
