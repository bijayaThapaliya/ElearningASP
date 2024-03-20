using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
	public class Progress
	{
		[Key]
		public int  Id { get; set; }
		[ForeignKey("Student")]
		public int StudentId { get; set; }
		public Student? Student { get; set; }
		[ForeignKey("Course")]
		public int CourseId { get; set; }
		public Course? Course { get; set; }
		[ForeignKey("Lesson")]
		public int LessonId { get; set; }
		public Lesson? Lesson { get; set; }
		public int LessonCompleted { get; set; }
	}
}

