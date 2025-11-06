using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.HelperMethods;
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
            // Don't forget that the data from dto might be null!!!
            if(newUser.Email == null || newUser.Email.Length == 0 ||
                newUser.Password == null || newUser.Password.Length == 0 ||
                newUser.Name == null || newUser.Name.Length == 0)
            {
                // if something goes wrong (Status code 400)
                return BadRequest(new { error = "Bad request" });
            }

            // Check, if the user already exists
            var FindUser = _context.User.Where(d => d.Email == newUser.Email).Count();

            if (FindUser != 0)
            {
                // Return with error code that user already exists (??? error code)
                return BadRequest(new { error = "User already exists with this email" });
            }

            // Create a hashed password
            string Hashedpwd = PasswordHash.PasswordHasher(newUser.Password);

            var user = new User()
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = Hashedpwd,
                Bio = "",
                ProfilePicture = new byte[] { },
                Role = false
            };

            // Add new user to database
            _context.User.Add(user);

            // Save changes
            _context.SaveChanges();

            // If all goes well (Status code 201)
            return Created(string.Empty, new { message = "User created" });
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromForm] DTO.LogInUser user)
        {
            // Don't forget that the data from dto might be null!!!
            if(user.Email == null || user.Email.Length == 0 || 
                user.Password == null || user.Password.Length == 0)
            {
                // Missing username/password (Status code 400)
                return BadRequest(new { error = "Email and password are required" });
            }

            // Check if user exists and if their password is matching
            var FindUser = _context.User.FirstOrDefault(d => d.Email == user.Email);
            if(FindUser == null || 
                FindUser.PasswordHash != PasswordHash.PasswordHasher(user.Password))
            {
                // Invalid credentials (Status code 401)
                return Unauthorized(new { error = "Invalid email or password" });
            }

            // Create a sessionID
            HttpContext.Session.SetInt32("UserID", FindUser.Id);

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

        [HttpGet("profile/profilepicture")]
        public IActionResult ReturnProfilePic()
        {
            // If user not found (Status code 404)
            return NotFound(new { error = "User not found" });

            // If preventing user from accessing image (Status code 401)
            return Unauthorized(new { error = "Unauthorized" });

            // If all goes well, return with image (Status code 200)
            byte[] Image = new byte[] { };
            return File(Image, "image/jpg");
        }

        [HttpPost("profile/profilepicture")]
        public IActionResult UpdateProfilePic([FromForm] DTO.UpdateProfilePicture ProfilePicture)
        {
            byte[] Image = new byte[] { };
            return File(Image, "image/jpg");
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
