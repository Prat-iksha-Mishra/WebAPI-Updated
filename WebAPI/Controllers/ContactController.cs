using Microsoft.AspNetCore.Mvc;
using static Api.Controllers.TrippifyController;
using System.Net;
using WebAPI.Models.Trippify;
using WebAPI.TrippifyData;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactController : Controller
    {
        public class Response
        {
            public string ResponseMessage { get; set; }
            public int ResponseStatus { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomerQuery([FromBody] ContactUs query)
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

            if (query == null)
            {
                response.ResponseMessage = "Invalid request body";
                response.ResponseStatus = (int)HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            // Validate the query object
            if (string.IsNullOrEmpty(query.UserName) || string.IsNullOrEmpty(query.UserEmail) || string.IsNullOrEmpty(query.UserComment))
            {
                response.ResponseMessage = "UserName, UserEmail, and UserComment are required.";
                response.ResponseStatus = (int)HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            query.CreatedOnUtc = DateTime.UtcNow;  // Set the CreatedOnUtc to the current UTC time

            ContactUsDb.AddCustomerQuery(query); // Add Query to your database

            response.ResponseMessage = "Cx Quest has been added successfully.";
            response.ResponseStatus = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        public List<ContactUs> GetAllQuery()
        {
            var query = ContactUsDb.GetAllCustomerQuery();
            return query;
        }

    }
}
