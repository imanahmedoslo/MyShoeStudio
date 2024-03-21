using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
using System.Security.Claims;

namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInfoControllers:ControllerBase
    {
        private MyShoeStudioDbContext _context;
        public PersonalInfoControllers(MyShoeStudioDbContext context)
        {
            _context = context;
        }
        [Authorize (Roles = "User,Admin")]
        [HttpPost("addPersonalInfo")]
        public async Task<IActionResult> AddPersonalInfo([FromBody] PersonalInfo personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.PersonalInfos.Add(personalInfo);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Personal Info added successfully" });
        }


        [Authorize(Roles = "User,Admin")]
        [HttpPut("updatePersonalInfo")]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] PersonalInfo personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.PersonalInfos.Update(personalInfo);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Personal Info updated successfully" });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("getPersonalInfo")]
        public async Task<IActionResult> GetPersonalInfo(string userId)
        {
            var personalInfo = await _context.PersonalInfos.FirstOrDefaultAsync(x => x.UserId == userId);
            if (personalInfo == null)
            {
                return NotFound();
            }
            return Ok(personalInfo);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllPersonalInfo")]
        public async Task<IActionResult> GetAllPersonalInfo()
        {
            var personalInfo = await _context.PersonalInfos.ToListAsync();
            if (personalInfo == null)
            {
                return NotFound();
            }
            return Ok(personalInfo);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("deletePersonalInfo/{id}")]
        public async Task<IActionResult> DeletePersonalInfo(int id)
        {
            var personalInfo = await _context.PersonalInfos.FindAsync(id);
            if (personalInfo == null)
            {
                return NotFound();
            }

            // Retrieve the current user's ID and role
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            // Check if the personalInfo belongs to the current user or if the user is an admin
            if (!isAdmin && personalInfo.UserId != currentUserId)
            {
                // If the user is not an admin and does not own the personalInfo, forbid the action
                return Forbid("You are not allowed to delete someone else's personal information.");
            }

            _context.PersonalInfos.Remove(personalInfo);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Personal Info deleted successfully" });
        }

    }
}
