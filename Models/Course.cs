using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class Course
	{
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        [InverseProperty("Course")]
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        [InverseProperty("Course")]
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        [InverseProperty("Course")]
        public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
    }
}

