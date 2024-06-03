using System.Data;

namespace WebAPI.Models.Trippify
{
    public class ContactUs
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserComment { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public ContactUs() { }
        public ContactUs(IDataReader reader)
        {
            Id = DBNull.Value != reader["Id"] ? (int)reader["Id"] : default(int);

            UserName = DBNull.Value != reader["UserName"] ? (string)reader["UserName"] : default(string);
            UserEmail = DBNull.Value != reader["UserEmail"] ? (string)reader["UserEmail"] : default(string);
            UserComment = DBNull.Value != reader["UserComment"] ? (string)reader["UserComment"] : default(string);
            CreatedOnUtc = DBNull.Value != reader["CreatedOnUtc"] ? (DateTime)reader["CreatedOnUtc"] : default(DateTime);
        }
    }
}
