using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recepttar.Server.Constants;
using Recepttar.Server.DTO.RecipeDTO;
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
            if (string.IsNullOrEmpty(newUser.Email) ||
                string.IsNullOrEmpty(newUser.Password) ||
                string.IsNullOrEmpty(newUser.Name))
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
                Rank = Enums.UserRanksEnum.Hobbi_szakács
            };

            // Add new user to database
            _context.User.Add(user);

            // Save changes
            _context.SaveChanges();

            // If all goes well (Status code 201)
            return Created(string.Empty, new { message = "User created" });
        }

        [HttpGet("checkEmail")]
        public IActionResult CheckEmail([FromQuery] string email)
        {
            // Validate input
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { error = "Bad request" });

            // Check if the email exists
            var exists = _context.User.Any(u => u.Email == email);

            if (exists)
            {
                // Email exists (Status code 200)
                return Ok(new { message = "Email exists" });
            }

            // Email does not exist (Status code 404)
            return NotFound(new { error = "Email not found" });
        }

        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromForm] DTO.LogInUser user)
        {
            // Don't forget that the data from dto might be null!!!
            if (string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Password))
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
            HttpContext.Session.SetInt32(SessionKeys.UserId, FindUser.Id);

            // Successful loggin (Status code 200)
            return Ok(new { message = "Successfully logged in" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // If already logged out (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            // Clears every session paramaters
            HttpContext.Session.Clear();

            // Clearing browser-side cookies
            Response.Cookies.Delete(".AspNetCore.Session");

            // Successful logout (Status code 200)
            return Ok(new { message = "Successfully logged out" });
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Prevent user from requesting profile data if not logged in
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            var UserData = new DTO.RequestProfileData()
            {
                Name = FindUser.Name,
                Bio = FindUser.Bio,
                ProfilePicture = "/user/" + ProfilePicturePath.Path,
                Rank = FindUser.Rank
            };
            // Successful request (Status code 200)
            return Ok(UserData);
        }

        [HttpPatch("profile")]
        public IActionResult UpdateProfile([FromForm] DTO.UpdateProfileData userUpdates)
        {
            // Unauthorized access (Status 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            // Track if anything was updated
            bool wasUpdated = false;

            if(!string.IsNullOrWhiteSpace(userUpdates.Name))
            {
                FindUser.Name = userUpdates.Name;
                wasUpdated = true;
            }

            if(!string.IsNullOrWhiteSpace(userUpdates.Email))
            {
                FindUser.Email = userUpdates.Email;
                wasUpdated = true;
            }

            if(!string.IsNullOrWhiteSpace(userUpdates.Password))
            {
                FindUser.PasswordHash = PasswordHash.PasswordHasher(userUpdates.Password);
                wasUpdated = true;
            }

            if(!string.IsNullOrWhiteSpace(userUpdates.Bio))
            {
                FindUser.Bio = userUpdates.Bio;
            }

            if (userUpdates.ProfilePicture != null)
            {
                using (var stream = new MemoryStream())
                {
                    userUpdates.ProfilePicture.CopyTo(stream);
                    FindUser.ProfilePicture = stream.ToArray();
                    wasUpdated = true;
                }
            }

            // Only save if something was actually updated
            if (wasUpdated)
            {
                _context.SaveChanges();

                return Ok(new { message = "User updated" });
            }
            else
            {
                // No changes were made
                return Ok(new { message = "No changes were made to the user" });
            }
        }

        [HttpGet("profile/{userId}")]
        public IActionResult GetOthersProfile(int userId)
        {
            var FindUser = _context.User.FirstOrDefault(d => d.Id == userId);

            if(FindUser == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // If found user, return profile data (Status code 200)
            var UserData = new DTO.RequestProfileData
            {
                Name = FindUser.Name,
                Bio = FindUser.Bio,
                ProfilePicture = "/user/" + ProfilePicturePath.Path + "/" + userId,
                Rank = FindUser.Rank
            };
            return Ok(UserData);
        }

        [HttpGet("profile/profilepicture")]
        public IActionResult ReturnProfilePic()
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            // If all goes well, return with image (Status code 200)
            byte[] Image = FindUser.ProfilePicture;
            return File(Image, "image/jpg");
        }


        [HttpGet("profile/profilepicture/{userID}")]
        public IActionResult ReturnProfilePic(int userId)
        {
            var FindUser = _context.User.FirstOrDefault(d => d.Id == userId);

            if (FindUser == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // If all goes well, return with image (Status code 200)
            byte[] Image = FindUser.ProfilePicture;
            return File(Image, "image/jpg");
        }

        #region User favorite
        [HttpGet("favorites")]
        public IActionResult GetUserFavorites()
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            var count = _context.Favorites.Count(f => f.UserId == UserId);

            var favorites = _context.Favorites
                .Where(f => f.UserId == UserId)
                .Select(f => new FavoriteRecipe()
                {
                    Id = f.RecipeId,
                    Title = f.Recipe.Title,
                    Difficulty = f.Recipe.Difficulty,
                    TimeMinutes = f.Recipe.TimeMinutes,
                    Servings = f.Recipe.Servings,
                    DishPicture = "recipes/" + f.RecipeId
                })
                .ToList();

            // Return with all the favorites (Status code 200)
            return Ok(favorites);
        }

        [HttpPost("favorites/{recipeId}")]
        public IActionResult AddToUserFavorites(int recipeId)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            var Recipe = _context.Recipe.FirstOrDefault(f => f.Id == recipeId);

            // If recipe not found (Status code 404)
            if(Recipe == null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            var existingFavorite = _context.Favorites.FirstOrDefault(f => f.UserId == UserId && f.RecipeId == recipeId);

            if (existingFavorite != null)
            {
                return Conflict(new { error = "Recipe already in favorites" });
            }

            _context.Favorites.Add(new Favorite
            {
                UserId = UserId.Value,
                RecipeId = recipeId
            });

            _context.SaveChanges();

            // Recipe successfully added to favorites (Status code 200)
            return Ok(new { message = "Recipe added to favorites" });
        }

        [HttpDelete("favorites/{recipeId}")]
        public IActionResult RemoveFromFavorites(int recipeId)
        {
            // Unauthorized access (Status code 401)
            if (!IsUserAuthorized.IsAuthorized(HttpContext))
            {
                return Unauthorized(new { error = "Unauthorized" });
            }

            int? UserId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            var FindUser = _context.User.FirstOrDefault(d => d.Id == UserId);

            var favorite = _context.Favorites.FirstOrDefault(f => f.UserId == UserId && f.RecipeId == recipeId);

            // If recipe not found (Status code 404)
            if (favorite == null)
            {
                return NotFound(new { error = "Recipe not in favorites" });
            }

            _context.Favorites.Remove(favorite);

            _context.SaveChanges();

            // Successful deletion (Status code 204)
            return NoContent();
        }
        #endregion User favorite
    }
}
