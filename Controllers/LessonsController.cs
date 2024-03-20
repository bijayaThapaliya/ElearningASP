using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELearning.Data;
using ELearning.Models;

namespace ELearning.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Lessons.Include(l => l.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        //GET: Lessons/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            //return View();

            // Retrieve the list of courses from the database
            var courses = await _context.Courses.ToListAsync();

            // Convert the list of courses to a SelectList
            SelectList courseList = new SelectList(courses, "Id", "Title");

            // Pass the SelectList to the view
            ViewData["CourseList"] = courseList;

            // Return the view for creating a new lesson
            return View();
        }

        //POST: Lessons/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CourseId")] Lesson lesson)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(lesson);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", lesson.CourseId);
            //return View(lesson);

            if (ModelState.IsValid)
            {
                // Add the new lesson to the database
                _context.Add(lesson);
                await _context.SaveChangesAsync();

                // Redirect to the lesson details page after successfully adding the lesson
                return RedirectToAction(nameof(Details), new { id = lesson.Id });
            }

            // If the model state is not valid, redisplay the form with validation errors
            // Retrieve the list of courses from the database
            var courses = await _context.Courses.ToListAsync();

            // Convert the list of courses to a SelectList
            SelectList courseList = new SelectList(courses, "Id", "Title");

            // Pass the SelectList to the view
            ViewData["CourseList"] = courseList;

            // Return the view with validation errors
            return View(lesson);


        }


        //public IActionResult Create(int courseId)
        //{
        //    ViewBag.CourseId = courseId;
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int courseId, Lesson lesson)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        lesson.CourseId = courseId;
        //        _context.Lessons.Add(lesson);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Details", "Course", new { id = courseId });
        //    }
        //    return View(lesson);
        //}



        






        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", lesson.CourseId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CourseId")] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", lesson.CourseId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ToggleCompletion(int lessonId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null)
            {
                return NotFound();
            }

            lesson.IsCompleted = !lesson.IsCompleted;
            await _context.SaveChangesAsync();

            var nextLessonId = await GetNextLessonId(lessonId);

            if(nextLessonId != null)
            {
                return RedirectToAction("Details", new { id = nextLessonId });
            }
            else
            {
                return RedirectToAction("CourseCompletion", new { courseId = lesson.CourseId });
            }
        }

        private async Task<int?> GetNextLessonId(int currentLessonId)
        {
            var nextLessonId = await _context.Lessons
                .Where(l => l.Id > currentLessonId)
                .OrderBy(l => l.Id)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();
            return nextLessonId;
        }


        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}
