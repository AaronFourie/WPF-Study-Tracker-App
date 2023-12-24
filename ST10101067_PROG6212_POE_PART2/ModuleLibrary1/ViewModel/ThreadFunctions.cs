using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModuleLibrary1.ViewModel
{
    public class ThreadFunctions
    {
        public void RetrievingData()
        {
            Thread.Sleep(3000);
        }
        public void VerifyingRecords()
        {
            Thread.Sleep(4000);
        }
        public void Loading()
        {
            Thread.Sleep(2500);
        }
    }
}
