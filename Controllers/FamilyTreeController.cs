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
    public class FamilyTreeController : ControllerBase
    {
        private readonly IFamilyTreeService _familyTreeService;

        public FamilyTreeController(IFamilyTreeService familyTreeService)
        {
            _familyTreeService = familyTreeService;
        }

        [HttpGet("{rootId}")]
        public async Task<ActionResult<FamilyTreeNodeDto>> GetFamilyTree(int rootId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var tree = await _familyTreeService.GetFamilyTreeAsync(rootId, userId);
                if (tree == null)
                {
                    return NotFound(new { message = "Family tree root member not found." });
                }

                return Ok(tree);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the family tree.", error = ex.Message });
            }
        }

        [HttpGet("roots")]
        public async Task<ActionResult<List<FamilyMemberDto>>> GetPotentialRoots()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var allMembers = await _familyTreeService.GetFamilyMembersAsync(userId);

                var potentialRoots = allMembers
                    .Where(m => m.FatherId == null && m.MotherId == null)
                    .OrderBy(m => m.DateOfBirth ?? DateTime.MaxValue)
                    .ToList();

                return Ok(potentialRoots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving potential tree roots.", error = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetFamilyStatistics()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var allMembers = await _familyTreeService.GetFamilyMembersAsync(userId);

                var statistics = new
                {
                    TotalMembers = allMembers.Count,
                    LivingMembers = allMembers.Count(m => m.IsAlive),
                    DeceasedMembers = allMembers.Count(m => !m.IsAlive),
                    MaleMembers = allMembers.Count(m => m.Gender?.ToLower() == "male"),
                    FemaleMembers = allMembers.Count(m => m.Gender?.ToLower() == "female")
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating family statistics.", error = ex.Message });
            }
        }
    }
}