using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG_2B_POE_PART_1.Connection
{
    public class DBConnection
    {
        public static SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\User\Downloads\ST10101067_PROG6212_POE_PART2\PROG 2B POE PART 2\StudentData.mdf"";Integrated Security=True");


        public void TestConnection()
        {
            try
            {
                if(con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                
            }
            catch(Exception a)
            {
                MessageBox.Show(a.Message);
            }
            
        }
    }
}
