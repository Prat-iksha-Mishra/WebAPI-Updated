using WebAPI.Models.Trippify;
using WebAPI.TrippifyData;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TrippifyController : Controller
	{
		public class Response
		{
			public string ResponseMessage { get; set; }
			public int ResponseStatus { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> AddLocation()
		{
			var response = new Response();
			string token = HttpContext.Request.Headers["Token"];
			if (string.IsNullOrEmpty(token))
			{
				response.ResponseMessage = "Token not Found";
				response.ResponseStatus = (int)HttpStatusCode.BadRequest;
				return BadRequest(response);
			}
			string user = HttpContext.Request.Query["User"];
			if (string.IsNullOrEmpty(user))
			{
				response.ResponseMessage = "User not Found";
				response.ResponseStatus = (int)HttpStatusCode.BadRequest;
				return BadRequest(response);
			}
			if (token != "123456")
			{
				response.ResponseMessage = "Token is invalid";
				response.ResponseStatus = (int)HttpStatusCode.BadRequest;
				return BadRequest(response);
			}

			if (!Request.HasFormContentType)
			{
				response.ResponseMessage = "Content type is not multipart/form-data";
				response.ResponseStatus = (int)HttpStatusCode.BadRequest;
				return BadRequest(response);
			}

			var form = await Request.ReadFormAsync();
			var location = new LocationModel
			{
				StateName = form["StateName"],
				LocationName = form["LocationName"],
				Address = form["Address"],
				ContactNumber = form["ContactNumber"],
				ContactPerson = form["ContactPerson"],
                Description = form["Description"],
                Category = form["Category"]
			};

			#region Upload Image

			var pictureFile = form.Files.GetFile("Picture");
			if (pictureFile != null && pictureFile.Length > 0)
			{
				string extension = Path.GetExtension(pictureFile.FileName);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pictureFile.FileName);
				string newFileName = $"{fileNameWithoutExtension}{extension}"; // Use original extension
				string directoryPath = "C:\\Users\\Atul\\Downloads\\Tripify\\Tripify\\Trippify\\src\\assets";
				string filePath = Path.Combine(directoryPath, newFileName);

				using (var ms = new MemoryStream())
				{
					await pictureFile.CopyToAsync(ms);
					ms.Position = 0; // Reset the memory stream position

					location.Picture = pictureFile.FileName;					
					using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
					{
						await ms.CopyToAsync(fileStream);
					}
				}
			}
			#endregion
			LocationDb.Add(location); // Add location to your database
			response.ResponseMessage = "Location has been added successfully.";
			response.ResponseStatus = (int)HttpStatusCode.OK;

			return Ok(response);
		}

        [HttpGet]
        public IActionResult GetLocationById(int id)
        {
            var location = LocationDb.GetLocationById(id);
            if (location != null)
            {
              return Ok(location); // Return OK status with locationModel
            }
            else
            {
                return NotFound(); // Return 404 Not Found if location is not found
            }
        }

       
        [HttpGet]
        public IActionResult Search(string place)
        {
            var location = LocationDb.SearchByLocation(place);
            if (location != null)
            {
                return Ok(location); // Return OK status with locationModel
            }
            else
            {
                return NotFound(); // Return 404 Not Found if location is not found
            }
        }

        [HttpPost]
		public IActionResult AddCategory()
		{
			Response response = new Response();
			Category category = new Category();
			string bd = string.Empty;
			if (HttpContext.Request.Body != null)
			{
				using (StreamReader stream = new StreamReader(Request.Body))
				{
					var body = stream.ReadToEndAsync().GetAwaiter();
					bd = body.GetResult();
				}
				if (string.IsNullOrEmpty(bd))
				{
					response.ResponseMessage = "Content is null";
					response.ResponseStatus = (int)HttpStatusCode.BadRequest;
					return BadRequest(response);
				}

				//var pictureFile = form..GetFile("Picture");
				//if (pictureFile != null && pictureFile.Length > 0)
				//{
				//	using (var ms = new MemoryStream())
				//	{
				//		await pictureFile.CopyToAsync(ms);
				//		location.Picture = ms.ToArray();
				//	}
				//}

				category = JsonConvert.DeserializeObject<Category>(bd);
				if (category != null)
				{
					LocationDb.AddCategory(category);
					response.ResponseMessage = "Category has been added succesfully.";
					response.ResponseStatus = (int)HttpStatusCode.OK;
				}
			}
			return Ok(response);
		}

		[HttpPost]
		public IActionResult AddState()
		{
			Response response = new Response();
			State state = new State();
			string bd = string.Empty;
			if (HttpContext.Request.Body != null)
			{
				using (StreamReader stream = new StreamReader(Request.Body))
				{
					var body = stream.ReadToEndAsync().GetAwaiter();
					bd = body.GetResult();
				}
				if (string.IsNullOrEmpty(bd))
				{
					response.ResponseMessage = "Content is null";
					response.ResponseStatus = (int)HttpStatusCode.BadRequest;
					return BadRequest(response);
				}
				state = JsonConvert.DeserializeObject<State>(bd);
				if (state != null)
				{
					LocationDb.AddState(state);
					response.ResponseMessage = "State has been added succesfully.";
					response.ResponseStatus = (int)HttpStatusCode.OK;
				}
			}
			return Ok(response);
		}

		[HttpGet]
		public List<State> GetAllState()
		{
			var states = LocationDb.GelAllState();
			return states;
		}

		[HttpGet]
		public List<Category> GetAllCategory()
		{
			var categories = LocationDb.GelAllCategory();
			return categories;
		}

		[HttpPost]

        public IActionResult DeleteLocation([FromBody] int id)
        {
            try
            {
                // Call the method to delete the location by its ID.
                bool deletionSuccess = LocationDb.DeleteLocation(id);

                // Check if the deletion was successful.
                if (deletionSuccess)
                {
                    // Return HTTP status code 200 (OK) with a success message.
                    return Ok("Location deleted successfully.");
                }
                else
                {
                    // Return HTTP status code 404 (Not Found) with an error message.
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed.
                return NotFound();
            }
        }


        [HttpPost]
        public IActionResult DeleteState([FromBody] int id)
        {
            try
            {
                // Call the method to delete the state by its ID.
                bool deletionSuccess = LocationDb.DeleteState(id);

                // Check if the deletion was successful.
                if (deletionSuccess)
                {
                    // Return HTTP status code 200 (OK) with a success message.
                    return Ok("State deleted successfully.");
                }
                else
                {
                    // Return HTTP status code 404 (Not Found) with an error message.
                    return NotFound("State not found or deletion failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed.
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult DeleteCategory([FromBody] int id)
        {
            try
            {
                // Call the method to delete the category by its ID.
                bool deletionSuccess = LocationDb.DeleteCategory(id);

                // Check if the deletion was successful.
                if (deletionSuccess)
                {
                    // Return HTTP status code 200 (OK) with a success message.
                    return Ok("Category deleted successfully.");
                }
                else
                {
                    // Return HTTP status code 404 (Not Found) with an error message.
                    return NotFound("Category not found or deletion failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed.
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet]
		public List<LocationModel> GelAllLocation()
		{
			var location = LocationDb.GelAllLocation();
			return location;
		}

		private readonly string baseDirectory = "C:\\Users\\Asus\\Downloads\\Tripify\\Tripify\\Trippify\\src\\assets";
	
	}
}
