using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTreeAPI.Models
{
    public class FamilyMember
    {
        [Key]
        public int Id { get; set; }

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

        // Foreign Keys
        [Required]
        public string UserId { get; set; } = string.Empty;

        public int? FatherId { get; set; }

        public int? MotherId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("FatherId")]
        public virtual FamilyMember? Father { get; set; }

        [ForeignKey("MotherId")]
        public virtual FamilyMember? Mother { get; set; }

        // Children relationship
        public virtual ICollection<FamilyMember> ChildrenAsFather { get; set; } = new List<FamilyMember>();

        public virtual ICollection<FamilyMember> ChildrenAsMother { get; set; } = new List<FamilyMember>();

        // Spouse relationships
        public virtual ICollection<Spouse> SpousesAsFirstMember { get; set; } = new List<Spouse>();

        public virtual ICollection<Spouse> SpousesAsSecondMember { get; set; } = new List<Spouse>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;

                var endDate = DateOfDeath ?? DateTime.UtcNow;
                var age = endDate.Year - DateOfBirth.Value.Year;

                if (endDate.DayOfYear < DateOfBirth.Value.DayOfYear)
                    age--;

                return age;
            }
        }

        [NotMapped]
        public bool IsAlive => !DateOfDeath.HasValue;
    }
}