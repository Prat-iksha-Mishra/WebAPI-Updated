using WebAPI.Models.Trippify;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Net;
using System.Data;

namespace WebAPI.TrippifyData
{

//	CREATE TABLE Location(
//		Id INT IDENTITY(1,1) PRIMARY KEY,
//		StateName VARCHAR(255),
//    LocationName VARCHAR(255),
//    Address VARCHAR(500),
//    ContactNumber VARCHAR(20),
//    ContactPerson VARCHAR(255),
//	Description VARCHAR(500),
//    Category VARCHAR(255),
//    Picture VARCHAR(255),
//    CreatedOnUtc DATETIME
//);




	public class LocationDb
	{
		public static void Add(LocationModel location)
		{
			SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand($"Insert into Location (StateName,LocationName,Address,ContactNumber,Description,ContactPerson,Category,Picture,CreatedOnUtc) Values(@StateName,@LocationName,@Address,@ContactNumber,@Description,@ContactPerson,@Category,@Picture,@CreatedOnUtc);", sqlConnection);
			cmd.Parameters.AddWithValue("StateName", location.StateName);
			cmd.Parameters.AddWithValue("LocationName", location.LocationName);
			cmd.Parameters.AddWithValue("Address", location.Address);
			cmd.Parameters.AddWithValue("ContactNumber", location.ContactNumber);
			cmd.Parameters.AddWithValue("Description", location.Description);
			cmd.Parameters.AddWithValue("ContactPerson", location.ContactPerson);
			cmd.Parameters.AddWithValue("Category", location.Category);
			cmd.Parameters.AddWithValue("Picture", location.Picture);
			cmd.Parameters.AddWithValue("CreatedOnUtc", DateTime.UtcNow);
			cmd.ExecuteNonQuery();

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

		public static void AddCategory(Category category)
		{
			SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand($"Insert into Category (Name,Description) Values(@Name,@Description);", sqlConnection);
			cmd.Parameters.AddWithValue("Name", category.Name);
			cmd.Parameters.AddWithValue("Description", category.Description);
			cmd.ExecuteNonQuery();

		}
		public static void AddState(State state)
		{
			SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand($"Insert into States (Name,Description) Values(@Name,@Description);", sqlConnection);
			cmd.Parameters.AddWithValue("Name", state.Name);
			cmd.Parameters.AddWithValue("Description", state.Description);
			cmd.ExecuteNonQuery();

		}

		public static List<State> GelAllState()
		{
			List<State> stateList = new List<State>();	
			SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand($"Select * from States", sqlConnection);
			IDataReader reader = cmd.ExecuteReader();
			State state = new State();
			while(reader.Read())
			{
				state = new State(reader);
				stateList.Add(state);
			}
			return stateList;
		}

		public static List<Category> GelAllCategory()
		{
			List<Category> categoryList = new List<Category>();
			SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand($"Select * from Category", sqlConnection);
			IDataReader reader = cmd.ExecuteReader();
			Category category = new Category();
			while (reader.Read())
			{
				category = new Category(reader);
				categoryList.Add(category);
			}
			return categoryList;
		}
        public static LocationModel GetLocationById(int id)
        {
            LocationModel location = null;
            SqlConnection sqlConnection = ConnectionFactory.sqlConnection();
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand($"Select * from Location where Id=@id", sqlConnection);
            cmd.Parameters.AddWithValue("Id", id);
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                location = new LocationModel(reader);
            }
            sqlConnection.Close();
            return location;
        }
        //        USE db_Pratiksha;
        //        GO
        //        CREATE PROCEDURE SearchLocations
        //    @Location NVARCHAR(100)=''
        //AS
        //BEGIN
        //    SELECT Id, LocationName, StateName, Address, ContactNumber, ContactPerson, Description, Category, Picture, CreatedOnUtc
        //    FROM(
        //        SELECT LocationName, StateName, Address, ContactNumber, ContactPerson, Description, Category, Picture, CreatedOnUtc, Id FROM Location
        //        UNION ALL
        //        SELECT Name, NULL, NULL, NULL, NULL, Description, NULL, NULL, NULL, Id FROM States
        //    ) AS CombinedResults
        //    WHERE LocationName LIKE '%' + @Location + '%';
        //END;
        //GO


        //-- Example execution of the stored procedure
        //EXEC SearchLocations @Location = 'Badrinath';
        //GO
        public static List<LocationModel> SearchByLocation(string place)
        {
            LocationModel location = null;
            List<LocationModel> LocationList = new List<LocationModel>();

            SqlConnection sqlConnection = ConnectionFactory.sqlConnection();

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("SearchLocations", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Location", place);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        location = new LocationModel(reader);
                        LocationList.Add(location);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return LocationList;
        }


        public static bool DeleteLocation(int id)
        {
            // Initialize a variable to track whether the deletion was successful.
            bool success = false;

            // Open a connection to the SQL database.
            using (SqlConnection sqlConnection = ConnectionFactory.sqlConnection())
            {
                sqlConnection.Open();

                // Create a SQL command to delete the location with the specified ID.
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Location WHERE Id=@id", sqlConnection))
                {
                    // Add a parameter to the SQL command to prevent SQL injection.
                    cmd.Parameters.AddWithValue("Id", id);

                    // Execute the SQL command.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were affected (indicating successful deletion).
                    success = rowsAffected > 0;
                }
            }

            // Return whether the deletion was successful.
            return success;
        }

        public static bool DeleteState(int id)
        {
            // Initialize a variable to track whether the deletion was successful.
            bool success = false;

            // Open a connection to the SQL database.
            using (SqlConnection sqlConnection = ConnectionFactory.sqlConnection())
            {
                sqlConnection.Open();

                // Create a SQL command to delete the location with the specified ID.
                using (SqlCommand cmd = new SqlCommand("DELETE FROM States WHERE Id=@id", sqlConnection))
                {
                    // Add a parameter to the SQL command to prevent SQL injection.
                    cmd.Parameters.AddWithValue("Id", id);

                    // Execute the SQL command.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were affected (indicating successful deletion).
                    success = rowsAffected > 0;
                }
            }

            // Return whether the deletion was successful.
            return success;
        }
        public static bool DeleteCategory(int id)
        {
            // Initialize a variable to track whether the deletion was successful.
            bool success = false;

            // Open a connection to the SQL database.
            using (SqlConnection sqlConnection = ConnectionFactory.sqlConnection())
            {
                sqlConnection.Open();

                // Create a SQL command to delete the location with the specified ID.
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE Id=@id", sqlConnection))
                {
                    // Add a parameter to the SQL command to prevent SQL injection.
                    cmd.Parameters.AddWithValue("Id", id);

                    // Execute the SQL command.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were affected (indicating successful deletion).
                    success = rowsAffected > 0;
                }
            }

            // Return whether the deletion was successful.
            return success;
        }

    }
}
