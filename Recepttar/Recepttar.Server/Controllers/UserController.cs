using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Models;
using System.Security.Cryptography;
using System.Text;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("user/")]
    public class UserController : Controller // ALWAYS INHERIT IF CONTROLLER
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromForm] DTO.RegisterUser newUser)
        {
            // if something goes wrong (Status code 400)
            // Don't forget that the data from dto might be null!!!
            return BadRequest(new { error = "Bad request" });

            // If all goes well (Status code 201)
            return Created(string.Empty, new { message = "User created" });
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromForm] DTO.LogInUser user)
        {
            // Don't forget that the data from dto might be null!!!

            // Missing username/password (Status code 400)
            return BadRequest(new { error = "Email and password are required" });

            // Invalid credentials (Status code 401)
            return Unauthorized(new { error = "Invalid email or password" });

            // Successful loggin (Status code 200)
            return Ok(new { message = "Successfully logged in", token = "TODO" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // If already logged out (Status code 401)
            return Unauthorized(new { message = "Unauthorized" });

            // Successful logout (Status code 200)
            return Ok(new { message = "Successfully logged out" });
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Prevent user from requesting profile data if not logged in
            return Unauthorized(new { error = "Unauthorized" });

            var UserData = new DTO.RequestProfileData()
            {
                Name = "",
                Bio = "",
                ProfilePicture = ""
            };
            // Successful request (Status code 200)
            return Ok(UserData);
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromForm] User user)
        {
            // Unauthorized access (Status 401)
            return Unauthorized(new { error = "Unauthorized" });

            // User not found (Status 404)
            return NotFound(new { error = "User not found" });

            // Successfully updated profile (200)
            var UserData = new DTO.RequestProfileData
            {
                Name = "",
                Bio = "",
                ProfilePicture = ""
            };
            return Ok(UserData);
        }

        [HttpGet("profile/{userId}")]
        public IActionResult GetOthersProfile(int userId)
        {
            // If requested user doesn't exists (Status code 404)
            return NotFound(new { error = "User not found", userId = userId});

            // If found user, return profile data (Status code 200)
            var UserData = new DTO.RequestProfileData
            {
                Name = "",
                Bio = "",
                ProfilePicture = ""
            };
            return Ok(UserData);
        }

        #region User favorite
        [HttpGet("favorites")]
        public IActionResult GetUserFavorites()
        {
            var EveryFavorite = new List<DTO.RecipeDTO.FavoritRecipe>();

            // Return with all the favorites (Status code 200)
            return Ok(EveryFavorite);
        }

        [HttpPost("favorites/{recipeId}")]
        public IActionResult AddToUserFavorites(int recipeId)
        {
            // If recipe not found (Status code 404)
            return NotFound(new { error = "Recipe not found" });

            // Recipe successfully added to favorites (Status code 200)
            return Ok(new { message = "Recipe added to favorites" });
        }
        #endregion User favorite
    }
}
