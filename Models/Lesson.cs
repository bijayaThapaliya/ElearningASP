using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class Lesson
	{
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }
        [InverseProperty("Lesson")]
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public bool IsCompleted { get; set; }
        public string? Content { get; set; }
    }
}

