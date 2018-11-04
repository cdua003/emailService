using System;
using System.Data.SqlClient;

namespace eMailService.Helps
{
    public class Helpers
    {
        private static readonly string DefaultConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EmailService.ConnectionString"].ConnectionString;

        public static SqlConnection NewConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(DefaultConnectionString);
            return new SqlConnection(builder.ConnectionString);
        }
    }
}