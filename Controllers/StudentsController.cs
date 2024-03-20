using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELearning.Data;
using ELearning.Models;
using ELearning.ViewModels;

namespace ELearning.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var studentsWithCourses = await _context.Students
        .Include(s => s.Enrollments) // Include enrollments
        .ThenInclude(e => e.Course)  // Include courses within enrollments
        .Select(student => new StudentWithCoursesViewModel
        {
            Sid=student.Id,
            Name = student.Name,
            ContactNumber=student.ContactNumber,
            DateOfBirth= student.DateOfBirth,
            Email=student.Email,
            Country=student.Country,
            CoursesEnrolled = student.Enrollments.Select(enrollment => enrollment.Course.Title).ToList(),
            Enrollments = student.Enrollments.ToList() // Include enrollments
        }).ToListAsync();

            return View(studentsWithCourses);

        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            //var viewModel = new EnrollmentViewModel
            //{
            //    Courses = _context.Courses.Select(c => new SelectListItem
            //    {
            //        Value = c.Id.ToString(),
            //        Text = c.Title
            //    }).ToList()
            //};
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            //return View();

            var viewModel = new StudentWithCoursesViewModel
            {
                // Populate other properties as needed
                Courses = _context.Courses
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title
            })
            .ToList()
            };

            return View(viewModel);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(StudentWithCoursesViewModel viewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var student = new Student
        //        {
        //            Name = viewModel.Name,
        //            ContactNumber = viewModel.ContactNumber,
        //            Email = viewModel.Email,
        //            DateOfBirth = viewModel.DateOfBirth,
        //            Country = viewModel.Country

        //        };
        //        _context.Students.Add(student);
        //        await _context.SaveChangesAsync(); // Save changes to assign StudentId

        //        var enrollment = new Enrollment
        //        {
        //            StudentId = student.Id,
        //            CourseId = viewModel.SelectedCourseId
        //        };
        //        _context.Enrollments.Add(enrollment);
        //        await _context.SaveChangesAsync(); // Save changes to create Enrollment

        //        return RedirectToAction(nameof(Index));
        //    }

        //    viewModel.Courses = _context.Courses
        //       .Select(c => new SelectListItem
        //       {
        //           Value = c.Id.ToString(),
        //           Text = c.Title
        //       }).ToList();

        //    return View(viewModel);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentWithCoursesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = viewModel.Name,
                    ContactNumber = viewModel.ContactNumber,
                    Email = viewModel.Email,
                    DateOfBirth = viewModel.DateOfBirth,
                    Country = viewModel.Country
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync(); // Save changes to assign StudentId

                // Iterate over the selected course IDs and create enrollments for each
                foreach (var courseId in viewModel.SelectedCourseIds)
                {
                    var enrollment = new Enrollment
                    {
                        StudentId = student.Id,
                        CourseId = courseId
                    };
                    _context.Enrollments.Add(enrollment);
                }
                await _context.SaveChangesAsync(); // Save changes to create all enrollments

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, repopulate the Courses property
            viewModel.Courses = _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }).ToList();

            return View(viewModel);
        }


        // GET: Students/Edit/5


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the student details from the database
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Retrieve the courses available for enrollment
            var courses = await _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }).ToListAsync();

            // Retrieve the courses in which the student is already enrolled
            var studentEnrollments = await _context.Enrollments
                .Where(e => e.StudentId == id)
                .Select(e => e.CourseId)
                .ToListAsync();

            // Initialize the view model with student details and enrolled courses
            var viewModel = new StudentWithCoursesViewModel
            {
                Student = student,
                Sid = student.Id,
                Name = student.Name,
                ContactNumber = student.ContactNumber,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Country = student.Country,
                CoursesEnrolled = studentEnrollments.Select(e => e.ToString()).ToList(),
                Courses = courses,
                SelectedCourseIds = studentEnrollments // Pre-select the enrolled courses
            };

            return View(viewModel);
        }


        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(student);
        //}






        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentWithCoursesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the student from the database
                var student = await _context.Students.FindAsync(viewModel.Sid);
                if (student == null)
                {
                    return NotFound();
                }

                // Update the student's details
                student.Name = viewModel.Name;
                student.ContactNumber = viewModel.ContactNumber;
                student.Email = viewModel.Email;
                student.DateOfBirth = viewModel.DateOfBirth;
                student.Country = viewModel.Country;

                // Update the student's enrollments
                // First, remove existing enrollments
                _context.Enrollments.RemoveRange(_context.Enrollments.Where(e => e.StudentId == viewModel.Sid));

                // Then, add new enrollments based on selected course IDs
                foreach (var courseId in viewModel.SelectedCourseIds)
                {
                    var enrollment = new Enrollment
                    {
                        StudentId = viewModel.Sid,
                        CourseId = courseId
                    };
                    _context.Enrollments.Add(enrollment);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, repopulate the Courses property
            viewModel.Courses = _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }).ToList();

            return View(viewModel);
        }






        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Student student)
        //{
        //    if (id != student.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(student);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!StudentExists(student.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
