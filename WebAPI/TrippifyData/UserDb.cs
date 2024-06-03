using System.Data.SqlClient;
using System.Security.Principal;
using WebAPI.Models.Trippify;

namespace WebAPI.TrippifyData
{
	public class UserDb
	{
//		CREATE TABLE TripifyUser(
//Id INT IDENTITY(1,1) PRIMARY KEY,
//Name NVARCHAR(50) NOT NULL,
//Email NVARCHAR(50) NOT NULL,
//Password NVARCHAR(50) NOT NULL,
//Role INT NOT NULL,
//CreatedOnUtc DATETIME NOT NULL
//);
		public static void Add(UserModel user)
		{
			try
			{
				using (SqlConnection sqlConnection = ConnectionFactory.sqlConnection())
				{
					sqlConnection.Open();
					SqlCommand cmd = new SqlCommand(
						@"INSERT INTO TripifyUser (Name,Email, Password, CreatedOnUtc) 
                VALUES (@Name, @Email, @Password, @CreatedOnUtc);", sqlConnection);
					cmd.Parameters.AddWithValue("@Name", user.Name);
					cmd.Parameters.AddWithValue("@Email", user.Email);
					cmd.Parameters.AddWithValue("@Password", user.Password);
					cmd.Parameters.AddWithValue("@CreatedOnUtc", DateTime.UtcNow);
					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine($"Exception occurred while adding user: {ex.Message}");
				// Optionally, rethrow the exception
				throw;
			}
		}

		public static UserModel GetUserByEmail(string email,string password)
		{
			UserModel user = null;
			using (SqlConnection connection = ConnectionFactory.sqlConnection())
			{
				string query = "SELECT * FROM TripifyUser WHERE Email = @Email And Password = @Password;";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Email", email);
				command.Parameters.AddWithValue("@Password", password);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while(reader.Read())
				{
					user = new UserModel
					{
						Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader["Id"]) : default(int),
						Name = DBNull.Value != reader["Name"] ? (string)reader["Name"] : default(string),
						Email = DBNull.Value != reader["Email"] ? (string)reader["Email"] : default(string),
						Password = DBNull.Value != reader["Password"] ? (string)reader["Password"] : default(string),
						CreatedOnUtc = DBNull.Value != reader["CreatedOnUtc"] ? (DateTime)reader["CreatedOnUtc"] : default(DateTime)
					};
				}
			}
			return user;
		}


	}
}
