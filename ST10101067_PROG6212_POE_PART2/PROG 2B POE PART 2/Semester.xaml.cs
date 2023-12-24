using ModuleLibrary1;
using PROG_2B_POE_PART_1.Connection;
using Syncfusion.Windows.Shared;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for Semester.xaml
    /// </summary>
    public partial class Semester : Window
    {
        public static Modules m = new Modules();
        public static bool IsErrors = true;

        public Semester()
        {
            DateTimeEdit dateTimeEdit = new DateTimeEdit();

            // Setting height and width to DateTimeEdit
            dateTimeEdit.Height = 25;
            dateTimeEdit.Width = 120;

            // Adding control into the main window
            this.Content = dateTimeEdit;

            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(WeeksTxtBox.Value > 0 && (DateTime)dateTimeEdit.DateTime != DateTime.MinValue)
                {
                    IsErrors = false;

                    m.Weeks = (int)WeeksTxtBox.Value;
                    m.StartDate = (DateTime)dateTimeEdit.DateTime;
                    m.EndDate = ((DateTime)dateTimeEdit.DateTime).AddDays((double)WeeksTxtBox.Value * 7);;

                    DBConnection db = new DBConnection();
                    db.TestConnection();

                    string add_semester_data = "INSERT INTO [dbo].[SEMESTER] (SEMESTER_WEEKS, SEMESTER_START_DATE, SEMESTER_END_DATE, STUDENT_USERNAME) VALUES (@SEMESTER_WEEKS, @SEMESTER_START_DATE, @SEMESTER_END_DATE, '" + Register.USERNAME + "')";
                    SqlCommand cmd1 = new SqlCommand(add_semester_data, DBConnection.con);

                    cmd1.Parameters.AddWithValue("@SEMESTER_WEEKS", WeeksTxtBox.Value);
                    cmd1.Parameters.AddWithValue("@SEMESTER_START_DATE", (DateTime)dateTimeEdit.DateTime);
                    cmd1.Parameters.AddWithValue("@SEMESTER_END_DATE", ((DateTime)dateTimeEdit.DateTime).AddDays((double)WeeksTxtBox.Value * 7));
                    cmd1.ExecuteNonQuery();

                    string title = "Success";
                    string prompt = "Semester Data saved into database correctly";
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DBConnection.con.Close();
                    MainWindow main = new MainWindow();
                    main.Show();
                    Close();
                }
                else if(WeeksTxtBox.Value <= 0)
                {
                    string title = "Invalid Weeks!";
                    string prompt = "There cannot be 0 or negative weeks in a semester: \n Weeks must be greater than 0";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
                
            
        }
    }
}
