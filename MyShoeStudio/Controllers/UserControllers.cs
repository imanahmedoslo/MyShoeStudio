using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyShoeStudio.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers:ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserControllers(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User { UserName = model.Email, Email = model.Email, IsAdmin = false };
            // Additional properties can be set here
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Instead of redirecting, return a success response
                return Ok(new { message = "User registered successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Instead of returning a view, return the validation problems
            return BadRequest(ModelState);
        }
    }

}


public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
     public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
     public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
     public string ConfirmPassword { get; set; }
}
