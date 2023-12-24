
namespace ModuleLibrary1.ViewModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SEMESTER
    {
        public int SEMESTER_ID { get; set; }
        public Nullable<int> SEMESTER_WEEKS { get; set; }
        public Nullable<System.DateTime> SEMESTER_START_DATE { get; set; }
        public Nullable<System.DateTime> SEMESTER_END_DATE { get; set; }
        public string STUDENT_USERNAME { get; set; }
    
        public virtual STUDENT STUDENT { get; set; }
    }
}
