using System.ComponentModel.DataAnnotations;

namespace FamilyTreeAPI.DTOs
{
    public class FamilyMemberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Biography { get; set; }
        public int? Age { get; set; }
        public bool IsAlive { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public List<SpouseDto> Spouses { get; set; } = new List<SpouseDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateFamilyMemberDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Biography { get; set; }

        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
    }

    public class UpdateFamilyMemberDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Biography { get; set; }

        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
    }

    public class SpouseDto
    {
        public int Id { get; set; }
        public int SpouseId { get; set; }
        public string SpouseName { get; set; } = string.Empty;
        public DateTime? MarriageDate { get; set; }
        public DateTime? DivorceDate { get; set; }
        public string? MarriageLocation { get; set; }
        public bool IsCurrentlyMarried { get; set; }
        public int? MarriageDurationYears { get; set; }
    }

    public class CreateSpouseDto
    {
        [Required]
        public int FirstMemberId { get; set; }

        [Required]
        public int SecondMemberId { get; set; }

        public DateTime? MarriageDate { get; set; }
        public DateTime? DivorceDate { get; set; }

        [MaxLength(500)]
        public string? MarriageLocation { get; set; }
    }

    public class FamilyTreeNodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? Age { get; set; }
        public bool IsAlive { get; set; }
        public List<FamilyTreeNodeDto> Children { get; set; } = new List<FamilyTreeNodeDto>();
        public List<SpouseDto> Spouses { get; set; } = new List<SpouseDto>();
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
    }
}