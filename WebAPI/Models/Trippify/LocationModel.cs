using System.Data;
using System.Xml.Linq;

namespace WebAPI.Models.Trippify
{
	public class LocationModel
	{
		public int Id { get ; set; }
		public string StateName { get ; set; }
		public string LocationName { get ; set; }
		public string Address { get ; set; }
		public string ContactNumber { get ; set; }
		public string ContactPerson { get ; set; }
		public string Description { get; set; }
		public string Category { get ; set; }
		public string Picture { get ; set; }
		public DateTime CreatedOnUtc { get ; set; }
		public LocationModel()
		{

		}
		public LocationModel(IDataReader reader)
		{
			Id = DBNull.Value != reader["Id"] ? (int)reader["Id"] : default(int);
			StateName = DBNull.Value != reader["StateName"] ? (string)reader["StateName"] : default(string);
			LocationName = DBNull.Value != reader["LocationName"] ? (string)reader["LocationName"] : default(string);
			Address = DBNull.Value != reader["Address"] ? (string)reader["Address"] : default(string);
			ContactNumber = DBNull.Value != reader["ContactNumber"] ? (string)reader["ContactNumber"] : default(string);
			ContactPerson = DBNull.Value != reader["ContactPerson"] ? (string)reader["ContactPerson"] : default(string);
            Description = DBNull.Value != reader["Description"] ? (string)reader["Description"] : default(string);
			Category = DBNull.Value != reader["Category"] ? (string)reader["Category"] : default(string);
			Picture = DBNull.Value != reader["Picture"] ? (string)reader["Picture"] : default(string);
			CreatedOnUtc = DBNull.Value != reader["CreatedOnUtc"] ? (DateTime)reader["CreatedOnUtc"] : default(DateTime);
		}
	}
}
