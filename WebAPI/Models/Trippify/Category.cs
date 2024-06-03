using System.Data;

namespace WebAPI.Models.Trippify
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Category()
		{

		}
		public Category(IDataReader reader)
		{
			Id = DBNull.Value != reader["Id"] ? (int)reader["Id"] : default(int);
			Name = DBNull.Value != reader["Name"] ? (string)reader["Name"] : default(string);
			Description = DBNull.Value != reader["Description"] ? (string)reader["Description"] : default(string);
		}
	}
}
