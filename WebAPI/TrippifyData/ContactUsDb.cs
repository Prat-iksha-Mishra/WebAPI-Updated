using System.Data;
using System.Data.SqlClient;
using WebAPI.Models.Trippify;

namespace WebAPI.TrippifyData
{
    public class ContactUsDb
    {
        public static void AddCustomerQuery(ContactUs contactUs)
        {
            SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into CustomerQueryTable (UserName,UserEmail,UserComment,CreatedOnUtc) Values(@UserName,@UserEmail,@UserComment,@CreatedOnUtc);", sqlConnection);
            cmd.Parameters.AddWithValue("UserName", contactUs.UserName);
            cmd.Parameters.AddWithValue("UserEmail", contactUs.UserEmail);
            cmd.Parameters.AddWithValue("UserComment", contactUs.UserComment);
            cmd.Parameters.AddWithValue("CreatedOnUtc", DateTime.UtcNow);
            cmd.ExecuteNonQuery();
        }

        public static List<ContactUs>GetAllCustomerQuery()
        {
            List<ContactUs> AllCustomerQuery = new List<ContactUs>();
            SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand($"Select * from CustomerQueryTable", sqlConnection);
            IDataReader reader = cmd.ExecuteReader();
            ContactUs query = new ContactUs();
            while (reader.Read())
            {
                query = new ContactUs(reader);
                AllCustomerQuery.Add(query);
            }
            return AllCustomerQuery;
        }

        public static List<LocationModel> GelAllLocation()
        {
            List<LocationModel> AllLocationList = new List<LocationModel>();
            SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand($"Select * from Location", sqlConnection);
            IDataReader reader = cmd.ExecuteReader();
            LocationModel location = new LocationModel();
            while (reader.Read())
            {
                location = new LocationModel(reader);
                AllLocationList.Add(location);
            }
            return AllLocationList;
        }
    }
}
