using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Models.Trippify;
using WebAPI.TrippifyData;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public class Response
        {
            public string ResponseMessage { get; set; }
            public int ResponseStatus { get; set; }
        }

		public class LoginRequest
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}

		[HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser()
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
            var newUser = new UserModel
            {
                Name = form["Name"],
                Email = form["Email"],
                Password = form["Password"],
            };
            UserDb.Add(newUser); // Add user to your database
            response.ResponseMessage = "User has been added successfully.";
            response.ResponseStatus = (int)HttpStatusCode.OK;

            return Ok(response);
        }

		[HttpPost]
		[Route("GetUserByEmailAndPassword")]
		public IActionResult GetUserByEmailAndPassword([FromBody] LoginRequest request)
		{
			try
			{
				// Check if email is provided
				if (string.IsNullOrEmpty(request.Email))
				{
					return BadRequest("Email is required");
				}

				// Retrieve user by email
				var user = UserDb.GetUserByEmail(request.Email, request.Password);

                Response response = new Response();
				// Check if user exists
				if(user != null)
				{
                    response.ResponseMessage = "Logged in Successfully";
                    response.ResponseStatus = (int)HttpStatusCode.OK;
					return Ok(response); // Return OK status with user object
				}
				else
				{
					response.ResponseMessage = "Email or Password does not match";
					response.ResponseStatus = (int)HttpStatusCode.NotFound;
					return Ok(response); // Return 404 Not Found if user is not found
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

	}
}
