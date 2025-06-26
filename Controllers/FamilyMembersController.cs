using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FamilyTreeAPI.Services;
using FamilyTreeAPI.DTOs;

namespace FamilyTreeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FamilyMembersController : ControllerBase
    {
        private readonly IFamilyTreeService _familyTreeService;

        public FamilyMembersController(IFamilyTreeService familyTreeService)
        {
            _familyTreeService = familyTreeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FamilyMemberDto>>> GetFamilyMembers()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var members = await _familyTreeService.GetFamilyMembersAsync(userId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving family members.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FamilyMemberDto>> GetFamilyMember(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var member = await _familyTreeService.GetFamilyMemberAsync(id, userId);
                if (member == null)
                {
                    return NotFound(new { message = "Family member not found." });
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the family member.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<FamilyMemberDto>> CreateFamilyMember(CreateFamilyMemberDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var member = await _familyTreeService.CreateFamilyMemberAsync(dto, userId);
                if (member == null)
                {
                    return BadRequest(new { message = "Failed to create family member. Please check parent relationships." });
                }

                return CreatedAtAction(nameof(GetFamilyMember), new { id = member.Id }, member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the family member.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FamilyMemberDto>> UpdateFamilyMember(int id, UpdateFamilyMemberDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var member = await _familyTreeService.UpdateFamilyMemberAsync(id, dto, userId);
                if (member == null)
                {
                    return NotFound(new { message = "Family member not found or failed to update." });
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the family member.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamilyMember(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var result = await _familyTreeService.DeleteFamilyMemberAsync(id, userId);
                if (!result)
                {
                    return BadRequest(new { message = "Family member not found or cannot be deleted (has children)." });
                }

                return Ok(new { message = "Family member deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the family member.", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<FamilyMemberDto>>> SearchFamilyMembers([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest(new { message = "Search term is required." });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var members = await _familyTreeService.SearchFamilyMembersAsync(searchTerm, userId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching family members.", error = ex.Message });
            }
        }

        [HttpPost("spouses")]
        public async Task<ActionResult<SpouseDto>> AddSpouse(CreateSpouseDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var spouse = await _familyTreeService.AddSpouseAsync(dto, userId);
                if (spouse == null)
                {
                    return BadRequest(new { message = "Failed to add spouse relationship. Please check member IDs or if relationship already exists." });
                }

                return Ok(spouse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the spouse relationship.", error = ex.Message });
            }
        }

        [HttpDelete("spouses/{spouseId}")]
        public async Task<IActionResult> RemoveSpouse(int spouseId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var result = await _familyTreeService.RemoveSpouseAsync(spouseId, userId);
                if (!result)
                {
                    return NotFound(new { message = "Spouse relationship not found." });
                }

                return Ok(new { message = "Spouse relationship removed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while removing the spouse relationship.", error = ex.Message });
            }
        }
    }
}