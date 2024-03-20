using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class Student
	{
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string?  Email { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        [InverseProperty("Student")]
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
        [InverseProperty("Student")]
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}

