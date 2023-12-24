using ModuleLibrary1;
using PROG_2B_POE_PART_1.Connection;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PROG_2B_POE_PART_1
{
    /// <summary>
    /// Interaction logic for Track_Work_Progess.xaml
    /// </summary>
    public partial class Track_Work_Progess : Window
    {

        public static StudyProgress study = new StudyProgress(); //instantiate StudyProgress class as new object
        public static ObservableCollection<StudyProgress> progress = new ObservableCollection<StudyProgress>(); //instantiate StudyProgress class as new observable collection
        public static ObservableCollection<StudyProgress> p2 = new ObservableCollection<StudyProgress>(); //instantiate StudyProgress class as new observable collection
        public static ObservableCollection<Data> g = new ObservableCollection<Data>();
        public static bool IsErrors = true;
        public static bool IsFilledIn = false;
        public static string title, prompt;
        public static ObservableCollection<double> tempVal = new ObservableCollection<double>();
        public static int count = 0;
        private SqlCommand objCommand;
        private SqlDataReader objDataReader;

        public static double credits { get; set; }
        public Track_Work_Progess()
        {

            InitializeComponent();

            DataContext = MainWindow.modules; //set datacontext to MainWindow.modules since it contains the modules list data
        }

        private void lstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = (Modules)lstBox.SelectedItem;
            credits = data.Credits;

        }


        private void SfTextBoxExt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Continue_Click(object sender, RoutedEventArgs e)//Method for 
        {
            GetErrors();
            if (IsErrors == false)
            {
                if (IsFilledIn == true)
                {
                    DBConnection db = new DBConnection();
                    db.TestConnection();

                    
                    SqlDataReader dr2 = new SqlCommand("SELECT SUM(HOURS_STUDIED) AS HOURS_STUDIED, MAX(CURRENT_WEEK) AS CURRENT_WEEK, MODULE_CODE, MIN(STUDY_HOURS_REMAINDING), MAX(SELF_STUDY_HOURS_PER_WEEK) AS STUDYS FROM STUDY WHERE STUDENT_USERNAME =  '" + Register.USERNAME + "' GROUP BY MODULE_CODE,CURRENT_WEEK", DBConnection.con).ExecuteReader();

                        while (dr2.Read())
                        {
                            
                        g.Add(new Data
                            {
                                //Sets Modules Class variables to user input
                                Code = dr2.GetString(2),
                                SelfStudyHours = (double)dr2.GetDecimal(4),
                                Hours = (double)dr2.GetDecimal(0),
                                Week = dr2.GetInt32(1),
                                StudyHoursRemainding = (double)dr2.GetDecimal(3),
                            });
                        }
                        WorkProgressList w = new WorkProgressList();
                        w.Show();
                        w.DataContext = g; //setting datacontext


                    Close();
                }
                    
                }


            

            else
            {
                title = "Data not saved";
                prompt = "Save the study session data and try again";
                MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    

        
        private void GetErrors()
        {

            try
            {
                var data = (Modules)lstBox.SelectedItem;
                if (data == null)
                {
                    MessageBox.Show("Error", "No Module Item Selected. Please select a module Item and try again", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                if (selectedDate.SelectedDate.Equals(""))
                {
                    title = "Error";
                    prompt = "No Date selected. Please select a date and try again";
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (startTime.SelectedTime.Equals(""))
                {
                    title = "Error";
                    prompt = "No start time selected. Please select a start time and try again";
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (endTime.SelectedTime.Equals(""))
                {
                    title = "Error";
                    prompt = "No end time selected. Please select an end time and try again";
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (startTime.SelectedTime >= endTime.SelectedTime)
                {
                    title = "Error";
                    prompt = "Invalid start time. Selected tart Time cannot be more than or equal to the selected end time";
                    MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (data != null && !selectedDate.SelectedDate.Equals("") && (DateTime)startTime.SelectedTime < (DateTime)endTime.SelectedTime)
                {
                    IsErrors = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Save_data_Click(object sender, RoutedEventArgs e)
        {
            GetErrors();
            if (IsErrors == false)
            {
                var data = (Modules)lstBox.SelectedItem;

                study.SelectedModule = data.Code;
                study.SelfStudyHours = data.SelfStudyHours;
                study.SelectedDate = (DateTime)selectedDate.SelectedDate;
                study.StartTime = (DateTime)startTime.SelectedTime;
                study.EndTime = (DateTime)endTime.SelectedTime;

                double hours = ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours;
                study.HoursWorked = hours;
                study.StudyHoursRemainding = data.SelfStudyHours - hours;


                    if (progress.Count > 1 && study.SelectedModule == data.Code)
                    {
                        study.TotalSTudyHours = 0.00;
                        for (int a = 0; a < progress.Count; a++)
                        {
                            study.TotalSTudyHours += study.TotalSTudyHours + progress[a].Hours;
                        }


                        p2.Add(new StudyProgress
                        {
                            StartDate = data.StartDate,
                            EndDate = data.EndDate,
                            Code = data.Code,
                            SelectedModule = data.Code,
                            SelectedDate = (DateTime)selectedDate.SelectedDate,
                            StartTime = (DateTime)startTime.SelectedTime,
                            EndTime = (DateTime)endTime.SelectedTime,
                            HoursWorked = ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours,
                            StudyHoursRemainding = study.SelfStudyHours - study.TotalSTudyHours,

                        });

                    }


                    else
                    {
                        progress.Add(new StudyProgress
                        {
                            StartDate = data.StartDate,
                            EndDate = data.EndDate,
                            Code = data.Code,
                            SelectedModule = data.Code,
                            SelectedDate = (DateTime)selectedDate.SelectedDate,
                            StartTime = (DateTime)startTime.SelectedTime,
                            EndTime = (DateTime)endTime.SelectedTime,
                            HoursWorked = ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours,

                            StudyHoursRemainding = data.SelfStudyHours - ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours,

                        });
                    }


                    try
                    {

                    DBConnection gh = new DBConnection();
                    gh.TestConnection();
                    if (progress.Count == 0)
                    {
                        DBConnection k = new DBConnection();
                        k.TestConnection();

                        string add_module_data = "INSERT INTO [dbo].[STUDY] (SELF_STUDY_HOURS_PER_WEEK, SELECTED_DATE, START_TIME, END_TIME, HOURS_STUDIED, STUDY_HOURS_REMAINDING, CURRENT_WEEK, MODULE_CODE, STUDENT_USERNAME) " +
                           "VALUES (@SELF_STUDY_HOURS_PER_WEEK, @SELECTED_DATE, @START_TIME, @END_TIME, @HOURS_WORKED, @STUDY_HOURS_REMAINDING, @CURRENT_WEEK, @SELECTED_MODULE, '" + Register.USERNAME + "')";
                        SqlCommand cmd0 = new SqlCommand(add_module_data, DBConnection.con);
                        Modules m = new Modules();
                        cmd0.Parameters.AddWithValue("@MODULE_CODE", data.Code);
                        cmd0.Parameters.AddWithValue("@SELECTED_MODULE", data.Code);
                        cmd0.Parameters.AddWithValue("@SELECTED_DATE", (DateTime)selectedDate.SelectedDate);
                        cmd0.Parameters.AddWithValue("@START_TIME", (DateTime)startTime.SelectedTime);
                        cmd0.Parameters.AddWithValue("@END_TIME", (DateTime)endTime.SelectedTime);
                        cmd0.Parameters.AddWithValue("@SELF_STUDY_HOURS_PER_WEEK", data.SelfStudyHours);
                        double weekDiff = ((DateTime)selectedDate.SelectedDate - (DateTime)data.StartDate).TotalDays / 7;
                        if (weekDiff <= 1)
                        {
                            weekDiff = 1;
                        }
                        else if (weekDiff > 1)
                        {
                            weekDiff = Math.Round((((DateTime)selectedDate.SelectedDate - (DateTime)data.StartDate).TotalDays) / 7, MidpointRounding.AwayFromZero);
                        }
                        string prompt3 = "Module: " + data.Code + "\n" + "Current week: " + weekDiff + "\n" + "Hours studied: " + ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours
                            + "\n" + "From " + (DateTime)startTime.SelectedTime + " to " + (DateTime)endTime.SelectedTime;
                        cmd0.Parameters.AddWithValue("@CURRENT_WEEK", weekDiff);
                        cmd0.Parameters.AddWithValue("@HOURS_WORKED", ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours);
                        cmd0.Parameters.AddWithValue("@STUDY_HOURS_REMAINDING", data.SelfStudyHours - ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours);
                        cmd0.ExecuteNonQuery();


                        string title2 = "Success";
                        string prompt2 = "Session Data saved into database correctly";
                        MessageBox.Show(prompt2, title2, MessageBoxButtons.OK, MessageBoxIcon.Information);


                        MessageBox.Show(prompt3, "Session info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (progress.Count > 0)
                    {

                        

                            string add_module_data = "INSERT INTO [dbo].[STUDY] (SELF_STUDY_HOURS_PER_WEEK, SELECTED_DATE, START_TIME, END_TIME, HOURS_STUDIED, STUDY_HOURS_REMAINDING, CURRENT_WEEK, MODULE_CODE, STUDENT_USERNAME) " +
                           "VALUES (@SELF_STUDY_HOURS_PER_WEEK, @SELECTED_DATE, @START_TIME, @END_TIME, @HOURS_WORKED, @STUDY_HOURS_REMAINDING, @CURRENT_WEEK, @SELECTED_MODULE, '" + Register.USERNAME + "')";

                            SqlCommand cmd7 = new SqlCommand(add_module_data, DBConnection.con);

                            Modules m = new Modules();
                            cmd7.Parameters.AddWithValue("@MODULE_CODE", data.Code);
                            cmd7.Parameters.AddWithValue("@SELECTED_MODULE", data.Code);
                            cmd7.Parameters.AddWithValue("@SELECTED_DATE", (DateTime)selectedDate.SelectedDate);
                            cmd7.Parameters.AddWithValue("@START_TIME", (DateTime)startTime.SelectedTime);
                            cmd7.Parameters.AddWithValue("@END_TIME", (DateTime)endTime.SelectedTime);
                            cmd7.Parameters.AddWithValue("@SELF_STUDY_HOURS_PER_WEEK", data.SelfStudyHours);
                            double weekDiff = ((DateTime)selectedDate.SelectedDate - (DateTime)data.StartDate).TotalDays / 7;
                            if (weekDiff > 0 && weekDiff <= 1)
                            {
                                weekDiff = 1;
                            }
                            else if (weekDiff > 1)
                            {
                                weekDiff = Math.Round((((DateTime)selectedDate.SelectedDate - (DateTime)data.StartDate).TotalDays) / 7, MidpointRounding.AwayFromZero);
                            }
                            string prompt3 = "Module: " + data.Code + "\n" + "Current week: " + weekDiff + "\n" + "Hours studied: " + ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours
                                + "\n" + "From " + (DateTime)startTime.SelectedTime + " to " + (DateTime)endTime.SelectedTime;
                            cmd7.Parameters.AddWithValue("@CURRENT_WEEK", weekDiff);
                            cmd7.Parameters.AddWithValue("@HOURS_WORKED", ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours);
                            cmd7.Parameters.AddWithValue("@STUDY_HOURS_REMAINDING", data.SelfStudyHours - ((DateTime)endTime.SelectedTime - (DateTime)startTime.SelectedTime).TotalHours);
                            cmd7.ExecuteNonQuery();


                            string title2 = "Success";
                            string prompt2 = "Session Data saved into database correctly";
                            MessageBox.Show(prompt2, title2, MessageBoxButtons.OK, MessageBoxIcon.Information);


                            MessageBox.Show(prompt3, "Session info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        objCommand = new SqlCommand("SELECT MAX(STUDY_ID) AS STUDY_ID FROM STUDY WHERE STUDENT_USERNAME =  '" + Register.USERNAME + "' AND MODULE_CODE = '" + data.Code + "' GROUP BY MODULE_CODE", DBConnection.con);
                        using (SqlDataReader objDataReader = objCommand.ExecuteReader())
                        {
                            {
                                while (objDataReader.Read())
                                {
                                    int id = objDataReader.GetInt32(0);


                                    string update_data = "UPDATE STUDY SET STUDY_HOURS_REMAINDING = SELF_STUDY_HOURS_PER_WEEK - (SELECT SUM(HOURS_STUDIED) FROM STUDY WHERE MODULE_CODE = '" + data.Code + "') WHERE STUDENT_USERNAME =  '" + Register.USERNAME + "' AND STUDY_ID = '" + id + "'";
                                    SqlCommand cmd3 = new SqlCommand(update_data, DBConnection.con);
                                    objDataReader.Close();
                                    cmd3.ExecuteNonQuery();
                                }
                            }

                            DBConnection.con.Close();

                        }
                    }





                    

                    }
                    catch (Exception ex)
                    {
                        string title = "Error";
                        string prompt = ex.Message.ToString();
                        MessageBox.Show(prompt, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        IsFilledIn = true;
                    }
                }

            }

        private void ResgisterFormHeading_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class Data
        {
            public string Code { set; get; }
            public double SelfStudyHours { set; get; }
            public double Hours { get; set; }
            public double StudyHoursRemainding { set; get; }
            public int Week { get; set; }
        }
    }
    }

    
    
    

