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
        Task<FamilyTreeNodeDto?> GetFamilyTreeAsync(int rootId, string userId);
        Task<List<FamilyMemberDto>> SearchFamilyMembersAsync(string searchTerm, string userId);
        Task<SpouseDto?> AddSpouseAsync(CreateSpouseDto dto, string userId);
        Task<bool> RemoveSpouseAsync(int spouseId, string userId);
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
                .Include(fm => fm.SpousesAsFirstMember)
                    .ThenInclude(s => s.SecondMember)
                .Include(fm => fm.SpousesAsSecondMember)
                    .ThenInclude(s => s.FirstMember)
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
                .Include(fm => fm.SpousesAsFirstMember)
                    .ThenInclude(s => s.SecondMember)
                .Include(fm => fm.SpousesAsSecondMember)
                    .ThenInclude(s => s.FirstMember)
                .FirstOrDefaultAsync();

            return member != null ? MapToDto(member) : null;
        }

        public async Task<FamilyMemberDto?> CreateFamilyMemberAsync(CreateFamilyMemberDto dto, string userId)
        {
            // Validate parent relationships
            if (dto.FatherId.HasValue)
            {
                var father = await _context.FamilyMembers
                    .FirstOrDefaultAsync(fm => fm.Id == dto.FatherId && fm.UserId == userId);
                if (father == null || father.Gender != "Male")
                    return null;
            }

            if (dto.MotherId.HasValue)
            {
                var mother = await _context.FamilyMembers
                    .FirstOrDefaultAsync(fm => fm.Id == dto.MotherId && fm.UserId == userId);
                if (mother == null || mother.Gender != "Female")
                    return null;
            }

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

            // Validate parent relationships
            if (dto.FatherId.HasValue && dto.FatherId != member.FatherId)
            {
                var father = await _context.FamilyMembers
                    .FirstOrDefaultAsync(fm => fm.Id == dto.FatherId && fm.UserId == userId);
                if (father == null || father.Gender != "Male")
                    return null;
            }

            if (dto.MotherId.HasValue && dto.MotherId != member.MotherId)
            {
                var mother = await _context.FamilyMembers
                    .FirstOrDefaultAsync(fm => fm.Id == dto.MotherId && fm.UserId == userId);
                if (mother == null || mother.Gender != "Female")
                    return null;
            }

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

            // Check if member has children
            var hasChildren = await _context.FamilyMembers
                .AnyAsync(fm => fm.FatherId == id || fm.MotherId == id);

            if (hasChildren)
                return false; // Cannot delete if has children

            _context.FamilyMembers.Remove(member);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<FamilyTreeNodeDto?> GetFamilyTreeAsync(int rootId, string userId)
        {
            var rootMember = await _context.FamilyMembers
                .Where(fm => fm.Id == rootId && fm.UserId == userId)
                .Include(fm => fm.SpousesAsFirstMember)
                    .ThenInclude(s => s.SecondMember)
                .Include(fm => fm.SpousesAsSecondMember)
                    .ThenInclude(s => s.FirstMember)
                .FirstOrDefaultAsync();

            if (rootMember == null)
                return null;

            return await BuildFamilyTreeNode(rootMember, userId);
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
                .Include(fm => fm.SpousesAsFirstMember)
                    .ThenInclude(s => s.SecondMember)
                .Include(fm => fm.SpousesAsSecondMember)
                    .ThenInclude(s => s.FirstMember)
                .OrderBy(fm => fm.FirstName)
                .ThenBy(fm => fm.LastName)
                .ToListAsync();

            return members.Select(MapToDto).ToList();
        }

        public async Task<SpouseDto?> AddSpouseAsync(CreateSpouseDto dto, string userId)
        {
            // Validate both members belong to user
            var firstMember = await _context.FamilyMembers
                .FirstOrDefaultAsync(fm => fm.Id == dto.FirstMemberId && fm.UserId == userId);
            var secondMember = await _context.FamilyMembers
                .FirstOrDefaultAsync(fm => fm.Id == dto.SecondMemberId && fm.UserId == userId);

            if (firstMember == null || secondMember == null)
                return null;

            // Check if relationship already exists
            var existingRelationship = await _context.Spouses
                .FirstOrDefaultAsync(s =>
                    (s.FirstMemberId == dto.FirstMemberId && s.SecondMemberId == dto.SecondMemberId) ||
                    (s.FirstMemberId == dto.SecondMemberId && s.SecondMemberId == dto.FirstMemberId));

            if (existingRelationship != null)
                return null;

            var spouse = new Spouse
            {
                FirstMemberId = dto.FirstMemberId,
                SecondMemberId = dto.SecondMemberId,
                MarriageDate = dto.MarriageDate,
                DivorceDate = dto.DivorceDate,
                MarriageLocation = dto.MarriageLocation
            };

            _context.Spouses.Add(spouse);
            await _context.SaveChangesAsync();

            return new SpouseDto
            {
                Id = spouse.Id,
                SpouseId = dto.SecondMemberId,
                SpouseName = secondMember.FullName,
                MarriageDate = spouse.MarriageDate,
                DivorceDate = spouse.DivorceDate,
                MarriageLocation = spouse.MarriageLocation,
                IsCurrentlyMarried = spouse.IsCurrentlyMarried,
                MarriageDurationYears = spouse.MarriageDurationYears
            };
        }

        public async Task<bool> RemoveSpouseAsync(int spouseId, string userId)
        {
            var spouse = await _context.Spouses
                .Include(s => s.FirstMember)
                .Include(s => s.SecondMember)
                .FirstOrDefaultAsync(s => s.Id == spouseId &&
                    (s.FirstMember.UserId == userId || s.SecondMember.UserId == userId));

            if (spouse == null)
                return false;

            _context.Spouses.Remove(spouse);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<FamilyTreeNodeDto> BuildFamilyTreeNode(FamilyMember member, string userId)
        {
            var children = await _context.FamilyMembers
                .Where(fm => (fm.FatherId == member.Id || fm.MotherId == member.Id) && fm.UserId == userId)
                .Include(fm => fm.SpousesAsFirstMember)
                    .ThenInclude(s => s.SecondMember)
                .Include(fm => fm.SpousesAsSecondMember)
                    .ThenInclude(s => s.FirstMember)
                .ToListAsync();

            var node = new FamilyTreeNodeDto
            {
                Id = member.Id,
                Name = member.FullName,
                FirstName = member.FirstName,
                LastName = member.LastName,
                DateOfBirth = member.DateOfBirth,
                DateOfDeath = member.DateOfDeath,
                Gender = member.Gender,
                ProfileImageUrl = member.ProfileImageUrl,
                Age = member.Age,
                IsAlive = member.IsAlive,
                FatherId = member.FatherId,
                MotherId = member.MotherId,
                Spouses = GetSpouseDtos(member),
                Children = new List<FamilyTreeNodeDto>()
            };

            foreach (var child in children)
            {
                var childNode = await BuildFamilyTreeNode(child, userId);
                node.Children.Add(childNode);
            }

            return node;
        }

        private FamilyMemberDto MapToDto(FamilyMember member)
        {
            return new FamilyMemberDto
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                FullName = member.FullName,
                DateOfBirth = member.DateOfBirth,
                DateOfDeath = member.DateOfDeath,
                Gender = member.Gender,
                ProfileImageUrl = member.ProfileImageUrl,
                Biography = member.Biography,
                Age = member.Age,
                IsAlive = member.IsAlive,
                FatherId = member.FatherId,
                MotherId = member.MotherId,
                FatherName = member.Father?.FullName,
                MotherName = member.Mother?.FullName,
                Spouses = GetSpouseDtos(member),
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
            };
        }

        private List<SpouseDto> GetSpouseDtos(FamilyMember member)
        {
            var spouses = new List<SpouseDto>();

            foreach (var spouse in member.SpousesAsFirstMember)
            {
                spouses.Add(new SpouseDto
                {
                    Id = spouse.Id,
                    SpouseId = spouse.SecondMemberId,
                    SpouseName = spouse.SecondMember.FullName,
                    MarriageDate = spouse.MarriageDate,
                    DivorceDate = spouse.DivorceDate,
                    MarriageLocation = spouse.MarriageLocation,
                    IsCurrentlyMarried = spouse.IsCurrentlyMarried,
                    MarriageDurationYears = spouse.MarriageDurationYears
                });
            }

            foreach (var spouse in member.SpousesAsSecondMember)
            {
                spouses.Add(new SpouseDto
                {
                    Id = spouse.Id,
                    SpouseId = spouse.FirstMemberId,
                    SpouseName = spouse.FirstMember.FullName,
                    MarriageDate = spouse.MarriageDate,
                    DivorceDate = spouse.DivorceDate,
                    MarriageLocation = spouse.MarriageLocation,
                    IsCurrentlyMarried = spouse.IsCurrentlyMarried,
                    MarriageDurationYears = spouse.MarriageDurationYears
                });
            }

            return spouses;
        }
    }
}