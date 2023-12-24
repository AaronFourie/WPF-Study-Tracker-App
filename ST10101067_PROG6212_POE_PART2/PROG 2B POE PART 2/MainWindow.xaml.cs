using ModuleLibrary1;
using PROG_2B_POE_PART_1.Connection;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String MODULE_CODE;
        public static int WEEKS;
        public static DateTime startDate, endDate;
        public bool filledIn, moduleSaved = false;
        public static TextBox[] ModuleInput = new TextBox[4];
        public static List<String> EmptyValueList = new List<String>();
        
        public static String[] TextBoxNames = { "Module code", "Module name", "Module credits", "Class hours per week" };
        
        public static ObservableCollection<Modules> modules = new ObservableCollection<Modules>();
        public MainWindow()
        {

            DateTimeEdit dateTimeEdit = new DateTimeEdit();

            // Setting height and width to DateTimeEdit
            dateTimeEdit.Height = 25;
            dateTimeEdit.Width = 120;

            // Adding control into the main window
            this.Content = dateTimeEdit;

            InitializeComponent();
           
        }

        private void Save_module_Click(object sender, RoutedEventArgs e)//Method that checks if form is filled in correctly and saves the module if it is.
        //Also displays a confirmation message to say that their module data was indeed recorded
        {

            GetErrors(); //Checks and spits out errors

            if (filledIn == true) //Once form has no erros and Weeks != 0 the condition will be met to save the data
            {
                DBConnection db = new DBConnection();
                db.TestConnection();

                SqlDataReader dr2 = new SqlCommand("SELECT SEMESTER_WEEKS, SEMESTER_START_DATE, SEMESTER_END_DATE FROM SEMESTER WHERE STUDENT_USERNAME = '" + Register.USERNAME + "' ",  DBConnection.con).ExecuteReader();
                while (dr2.Read())
                {
                    WEEKS = dr2.GetInt32(0);
                    startDate = dr2.GetDateTime(1);
                    endDate = dr2.GetDateTime(2);
                }
                
                
                    modules.Add(new Modules
                    {
                        //Sets Modules Class variables to user input
                        Code = (String)moduleCodeTxtBox.Text,
                        Weeks = (double)WEEKS,
                        Name = (String)ModuleNameTxtbox.Text,
                        Credits = (double)ModuleCreditsTxtbox.Value,
                        Hours = (double)ClassHoursPerWeekTxtbox.Value,
                        StartDate = startDate,
                        EndDate = endDate,
                        SelfStudyHours = (((double)ModuleCreditsTxtbox.Value * 10) / WEEKS) - (double)ClassHoursPerWeekTxtbox.Value
                        
                    }) ;
                    string title = "Form Sucess!";
                    string prompt = "Module Data Saved Succefully: \n\n";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Asterisk);
                    dr2.Close();
                    try
                {

                    if (moduleCodeTxtBox.Text.Length > 0 && ModuleNameTxtbox.Text.Length > 0 && ModuleCreditsTxtbox.Value > 0 && ClassHoursPerWeekTxtbox.Value > 0)
                    {
                       
                        string add_module_data = "INSERT INTO [dbo].[MODULE] (MODULE_CODE, MODULE_NAME, MODULE_CREDITS, CLASS_HOURS_PER_WEEK, STUDENT_USERNAME) VALUES (@MODULE_CODE, @MODULE_NAME, @MODULE_CREDITS, @CLASS_HOURS_PER_WEEK, '"+Register.USERNAME+"')";
                        SqlCommand cmd = new SqlCommand(add_module_data, DBConnection.con);

                        cmd.Parameters.AddWithValue("@MODULE_CODE", moduleCodeTxtBox.Text);
                        cmd.Parameters.AddWithValue("@MODULE_NAME", ModuleNameTxtbox.Text);
                        cmd.Parameters.AddWithValue("@MODULE_CREDITS", ModuleCreditsTxtbox.Value);
                        cmd.Parameters.AddWithValue("@CLASS_HOURS_PER_WEEK", ClassHoursPerWeekTxtbox.Value);
                        cmd.ExecuteNonQuery();

                        title = "Success";
                        prompt = "Module Data saved into database correctly";
                        MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DBConnection.con.Close();

                        ModuleInput[0] = moduleCodeTxtBox;
                        ModuleInput[1] = ModuleNameTxtbox;
                        ModuleInput[2] = ModuleCreditsTxtbox;
                        ModuleInput[3] = ClassHoursPerWeekTxtbox;
                        for (int i = 0; i < ModuleInput.Length; i++)
                        {
                            ModuleInput[i].IsReadOnly = false;
                            ModuleInput[i].Clear();
                        }

                        moduleSaved = true;



                    }

                }
                catch (Exception ex)
                {
                    title = "Error";
                    prompt = ex.Message.ToString();
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }


            else if (filledIn != true)
            {
                string title = "Form Error!";
                string prompt = "Module Data was not saved: \n\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
            }
            


        }    

        private void GetErrors()//Method for getting any errors in Module Form and if there are errors it wwill display the error or warning to the user
        //so they know exactly what to fill in in order to continue
        {
            TextBox[] ModuleInput = {moduleCodeTxtBox, ModuleNameTxtbox, ModuleCreditsTxtbox, ClassHoursPerWeekTxtbox};
            List<String> EmptyValueList = new List<String>();
            String[] TextBoxNames = { "Module code", "Module name", "Module credits", "Class hours per week", "Weeks", "Start Date" };
            for (int i = 0; i < ModuleInput.Length; i++)
            {
                if (string.IsNullOrEmpty(ModuleInput[i].Text))
                {
                    EmptyValueList.Add(TextBoxNames[i]);
                }
                
            }
            if (EmptyValueList.Count > 0)
            {
                string s = String.Join(Environment.NewLine, EmptyValueList);
                string title = "Form Error!";
                string prompt = "The following amounts cannot be left blank: \n\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt + s, title, buttons, MessageBoxIcon.Error);
            }
            if(ModuleCreditsTxtbox.Value <= 0)
            {
                string title = "Invalid number of module credits!";
                string prompt = "Module credits must be greater than 0: \n\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
            }
            if(ClassHoursPerWeekTxtbox.Value <= 0)
            {
                string title = "Invalid number of Class hours per week!";
                string prompt = "Class hours per week must be greater than 0: \n\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
            }
            if (EmptyValueList.Count == 0 && ModuleCreditsTxtbox.Value > 0 && ClassHoursPerWeekTxtbox.Value > 0)
            {
                
                filledIn = true;
            }
        }

        private void ModuleFormHeading_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {


            if (moduleSaved == true)
            {              
                ModuleList l = new ModuleList();
                l.Show();
                Close();
            }
            else
            {
                GetErrors();

                string title = "Form Comletion Error!";
                string prompt = "At least one module of data must be saved to continue: \n\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(prompt, title, buttons, MessageBoxIcon.Error);
            }
            
        }
    }
}
