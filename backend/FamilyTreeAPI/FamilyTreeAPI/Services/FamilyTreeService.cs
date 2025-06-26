using Microsoft.EntityFrameworkCore;
using FamilyTreeAPI.Data;
using FamilyTreeAPI.Models;
using FamilyTreeAPI.DTOs;

namespace FamilyTreeAPI.Services
{
    public interface IFamilyTreeService
    {
        Task<List<FamilyMemberDto>> GetFamilyMembersAsync(string userId);
        Task<FamilyMemberDto?> GetFamilyMemberAsync(int id, string userId);
        Task<FamilyMemberDto?> CreateFamilyMemberAsync(CreateFamilyMemberDto dto, string userId);
        Task<FamilyMemberDto?> UpdateFamilyMemberAsync(int id, UpdateFamilyMemberDto dto, string userId);
        Task<bool> DeleteFamilyMemberAsync(int id, string userId);
        Task<List<FamilyMemberDto>> SearchFamilyMembersAsync(string searchTerm, string userId);
    }

    public class FamilyTreeService : IFamilyTreeService
    {
        private readonly FamilyTreeContext _context;

        public FamilyTreeService(FamilyTreeContext context)
        {
            _context = context;
        }

        public async Task<List<FamilyMemberDto>> GetFamilyMembersAsync(string userId)
        {
            var members = await _context.FamilyMembers
                .Where(fm => fm.UserId == userId)
                .Include(fm => fm.Father)
                .Include(fm => fm.Mother)
                .OrderBy(fm => fm.FirstName)
                .ThenBy(fm => fm.LastName)
                .ToListAsync();

            return members.Select(MapToDto).ToList();
        }

        public async Task<FamilyMemberDto?> GetFamilyMemberAsync(int id, string userId)
        {
            var member = await _context.FamilyMembers
                .Where(fm => fm.Id == id && fm.UserId == userId)
                .Include(fm => fm.Father)
                .Include(fm => fm.Mother)
                .FirstOrDefaultAsync();

            return member != null ? MapToDto(member) : null;
        }

        public async Task<FamilyMemberDto?> CreateFamilyMemberAsync(CreateFamilyMemberDto dto, string userId)
        {
            var member = new FamilyMember
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                DateOfDeath = dto.DateOfDeath,
                Gender = dto.Gender,
                ProfileImageUrl = dto.ProfileImageUrl,
                Biography = dto.Biography,
                FatherId = dto.FatherId,
                MotherId = dto.MotherId,
                UserId = userId
            };

            _context.FamilyMembers.Add(member);
            await _context.SaveChangesAsync();

            return await GetFamilyMemberAsync(member.Id, userId);
        }

        public async Task<FamilyMemberDto?> UpdateFamilyMemberAsync(int id, UpdateFamilyMemberDto dto, string userId)
        {
            var member = await _context.FamilyMembers
                .FirstOrDefaultAsync(fm => fm.Id == id && fm.UserId == userId);

            if (member == null)
                return null;

            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;
            member.DateOfBirth = dto.DateOfBirth;
            member.DateOfDeath = dto.DateOfDeath;
            member.Gender = dto.Gender;
            member.ProfileImageUrl = dto.ProfileImageUrl;
            member.Biography = dto.Biography;
            member.FatherId = dto.FatherId;
            member.MotherId = dto.MotherId;
            member.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetFamilyMemberAsync(id, userId);
        }

        public async Task<bool> DeleteFamilyMemberAsync(int id, string userId)
        {
            var member = await _context.FamilyMembers
                .FirstOrDefaultAsync(fm => fm.Id == id && fm.UserId == userId);

            if (member == null)
                return false;

            _context.FamilyMembers.Remove(member);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<FamilyMemberDto>> SearchFamilyMembersAsync(string searchTerm, string userId)
        {
            var members = await _context.FamilyMembers
                .Where(fm => fm.UserId == userId &&
                    (fm.FirstName.Contains(searchTerm) ||
                     fm.LastName.Contains(searchTerm) ||
                     (fm.FirstName + " " + fm.LastName).Contains(searchTerm)))
                .Include(fm => fm.Father)
                .Include(fm => fm.Mother)
                .OrderBy(fm => fm.FirstName)
                .ThenBy(fm => fm.LastName)
                .ToListAsync();

            return members.Select(MapToDto).ToList();
        }

        private FamilyMemberDto MapToDto(FamilyMember member)
        {
            return new FamilyMemberDto
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                DateOfBirth = member.DateOfBirth,
                DateOfDeath = member.DateOfDeath,
                Gender = member.Gender,
                ProfileImageUrl = member.ProfileImageUrl,
                Biography = member.Biography,
                FatherId = member.FatherId,
                MotherId = member.MotherId,
                FatherName = member.Father?.FullName,
                MotherName = member.Mother?.FullName,
                Age = member.Age,
                IsAlive = member.IsAlive,
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
            };
        }
    }
}