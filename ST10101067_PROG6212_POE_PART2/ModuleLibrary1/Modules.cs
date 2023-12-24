using System;

namespace ModuleLibrary1
{
    public class Modules: Student //Module class that gets and sets module code, Name, Credits, hours weeks, startdate, enddate,
                                                  //and calculates selfstudyhours
     {
           private String code;
           public String Code
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
           private string name;
           public String Name {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnPropertyChanged();
                    }
                }
           }

           private double credits;
           public double Credits {
                get { return credits; }
                set
                {
                    if (credits != value)
                    {
                        credits = value;
                        OnPropertyChanged();
                    }
                }
           }

           private double hours;
           public double Hours {
                get { return hours; }
                set
                {
                    if (hours != value)
                    {
                        hours = value;
                        OnPropertyChanged();
                    }
                }
           }
        private double weeks;
        public double Weeks
        {
            get { return weeks; }
            set
            {
                if (weeks != value)
                {
                    weeks = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime startdate;
        public DateTime StartDate
        {
            get { return startdate; }
            set
            {
                if (startdate != value)
                {
                    startdate = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime enddate;
        public DateTime EndDate
        {
            set
            {
                enddate = startdate.AddDays(Weeks*7);
            }
            get { return enddate; }

        }
        private double selfstudyhours;
        public double SelfStudyHours 
        {
            set
            {
                selfstudyhours = ((Credits * 10) / Weeks) - Hours;
            }
            get { return selfstudyhours; }

        }

        public double TotalSTudyHours { get; set; }





    }
}

