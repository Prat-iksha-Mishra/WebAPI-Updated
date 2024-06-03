using System.Data;

namespace WebAPI.Models.Trippify
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public UserModel() { }

        public UserModel(IDataReader reader) 
        {
            Id = DBNull.Value != reader["Id"] ? (int)reader["Id"] : default(int);
            Name = DBNull.Value != reader["Name"] ? (string)reader["Name"] : default(string);
            Email = DBNull.Value != reader["Email"] ? (string)reader["Email"] : default(string);
            Password = DBNull.Value != reader["Password"] ? (string)reader["Password"] : default(string);
            CreatedOnUtc = DBNull.Value != reader["CreatedOnUtc"] ? (DateTime)reader["CreatedOnUtc"] : default(DateTime);
           
        }
    }
}
