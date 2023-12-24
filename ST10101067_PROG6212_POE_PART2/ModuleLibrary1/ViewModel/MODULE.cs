
namespace ModuleLibrary1.ViewModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class MODULE
    {
        public int MODULE_ID { get; set; }
        public string MODULE_CODE { get; set; }
        public string MODULE_NAME { get; set; }
        public Nullable<decimal> MODULE_CREDITS { get; set; }
        public Nullable<decimal> CLASS_HOURS_PER_WEEK { get; set; }
        public string STUDENT_USERNAME { get; set; }
    
        public virtual STUDENT STUDENT { get; set; }
    }
}
