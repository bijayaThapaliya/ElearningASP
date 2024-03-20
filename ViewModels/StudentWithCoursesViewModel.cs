using System;
using ELearning.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearning.ViewModels
{
	public class StudentWithCoursesViewModel
	{
        public Student? Student { get; set; }
        public int Sid { get; set; }
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public List<string>? CoursesEnrolled { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
        public int SelectedCourseId { get; set; }
        //public Course Courses { get; set; }
        public List<SelectListItem>? Courses { get; set; }
        public List<int>? SelectedCourseIds { get; set; }
    }
}

