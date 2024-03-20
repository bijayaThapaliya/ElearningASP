using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class TeacherCourse
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("Teacher")]
		public int TeacherId { get; set; }
		public Teacher? Teacher { get; set; }
		[ForeignKey("Course")]
		public int CourseId { get; set; }
		public Course? Course { get; set; }
	}
}

