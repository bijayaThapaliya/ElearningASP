using System;
using ELearning.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearning.ViewModels
{
    public class EnrollmentViewModel
    {
        public string? StudentName { get; set; }
        public int SelectedCourseId { get; set; } // New property for the selected course ID
        public List<SelectListItem>? Courses
        {
            get; set;
        }
        public Course? Course { get; set; }
    }
}

