using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace UtilityLayer
{

    //ANS 8
    public class DBUtil
    {
        //public static SqlConnection GetDBConn()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile()
        //    string connectionString = "Data Source=KHUSHI\\SQLEXPRESS;Initial Catalog=CodingChallengeDB;Integrated Security=True;TrustServerCertificate=True"; // Corrected connection string
        //    return new SqlConnection(connectionString);
        //}

        private static IConfigurationRoot _configuration;
        static string s = null;
        static DBUtil()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Khushi new\\Hexaware Training\\Assessments\\Khushi C# Coding Challenge\\UtilityLayer\\appsettings.json",
                optional: true, reloadOnChange: true);
            _configuration = builder.Build();
                
        }
        public static string ReturnCn(String key)
        {
            s = _configuration.GetConnectionString("dbCn");
            return s;
        }
    }



}
