using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class Teacher
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }

		[InverseProperty("Teacher")]
		public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
	}
}

