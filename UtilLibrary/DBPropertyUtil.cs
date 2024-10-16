using Microsoft.Data.SqlClient;

namespace UtilLibrary
{
    public class DBPropertyUtil
    {
        static string cnstring = @"Data Source=.\sqlexpress;Initial Catalog=TechShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public static SqlConnection GetConnectionString()
        {
            SqlConnection cn = new SqlConnection(cnstring);

            return cn;
        }

    }
}
