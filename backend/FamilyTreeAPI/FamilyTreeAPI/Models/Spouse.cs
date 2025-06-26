using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTreeAPI.Models
{
    public class Spouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FirstMemberId { get; set; }

        [Required]
        public int SecondMemberId { get; set; }

        public DateTime? MarriageDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        [MaxLength(500)]
        public string? MarriageLocation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("FirstMemberId")]
        public virtual FamilyMember FirstMember { get; set; } = null!;

        [ForeignKey("SecondMemberId")]
        public virtual FamilyMember SecondMember { get; set; } = null!;

        // Computed Properties
        [NotMapped]
        public bool IsCurrentlyMarried => MarriageDate.HasValue && !DivorceDate.HasValue;

        [NotMapped]
        public int? MarriageDurationYears
        {
            get
            {
                if (!MarriageDate.HasValue) return null;

                var endDate = DivorceDate ?? DateTime.UtcNow;
                var duration = endDate.Year - MarriageDate.Value.Year;

                if (endDate.DayOfYear < MarriageDate.Value.DayOfYear)
                    duration--;

                return duration;
            }
        }
    }
}