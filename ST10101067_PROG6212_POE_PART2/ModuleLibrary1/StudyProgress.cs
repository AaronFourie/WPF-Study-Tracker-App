using System;

namespace ModuleLibrary1
{
    public class StudyProgress: Modules//Module class that edxtends Modules class since it need the SelfStudy hours.
                                       //This class contains getters and setters for module code, the SelectedModule, the SelectedDate, StartTime, EndTime, HoursWorked,
                                       //and calculates the StudyHoursRemaindin using the SelfSTudyHours calculated from the modules classs
    {
        private String code;
        public new String Code
        {
            get { return code; }
            set
            {
                if (code != value)
                {
                    code = value;
                    OnPropertyChanged();
                }
            }
        }

        private String selectedModule;
        public String SelectedModule
        {
            get { return selectedModule; }
            set
            {
                if (selectedModule != value)
                {
                    selectedModule = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged();
                }
            }
        }
        private double hoursWorked;
        public double HoursWorked
        {
            //Weeks = Start
           
            set
            {
                if (Code != SelectedModule)
                {
                    hoursWorked = 0.00;
                }
                else
                {
                    hoursWorked = ((DateTime)EndTime - (DateTime)StartTime).TotalHours;
                }
                
            }
            get { return hoursWorked; }

        }


        private double studyHoursRemainding;
        public double StudyHoursRemainding
        {
            set
            {
                
                studyHoursRemainding =  SelfStudyHours - HoursWorked;
            }
            get { return studyHoursRemainding; }
        }
        private double totalStudyHours;
        public double TotalSTudyHours
        {
            set
            {

                totalStudyHours += hoursWorked;
            }
            get { return totalStudyHours; }
        }

    }
}
