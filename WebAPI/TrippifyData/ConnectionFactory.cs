using System.Data.SqlClient;

namespace WebAPI.TrippifyData
{
    public class ConnectionFactory
    {
        public static SqlConnection sqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection("Server=(localdb)\\Local;Database=db_Pratiksha;Trusted_Connection=True;TrustServerCertificate=True");
            return sqlConnection;
        }
    }
}
