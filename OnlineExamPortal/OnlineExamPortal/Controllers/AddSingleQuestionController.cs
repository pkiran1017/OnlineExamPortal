using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineExamPortal.Models;

namespace OnlineExamPortal.Controllers
{
    public class AddSingleQuestionController : Controller
    {
        private readonly ONLINEEXAMPORTALContext _context;

        public AddSingleQuestionController(ONLINEEXAMPORTALContext context)
        {
            _context = context;
        }
        // GET: AddSingleQuestion
        
        public async Task<IActionResult> Index(int? selectedTopic)
        {
            ViewBag.Topics = await _context.Topics.ToListAsync();

            // Fetch questions based on the selected topic (if any)
            IQueryable<Question> questionsQuery = _context.Questions.Include(q => q.Topic);

            if (selectedTopic.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.TopicId == selectedTopic.Value);
            }

            var questions = await questionsQuery.ToListAsync();

            var highestExamId = await _context.Exams.MaxAsync(e => (int?)e.ExamId) ?? 0;

            ViewBag.HighestExamId = highestExamId;
            var userCount = await _context.Users.CountAsync(u => u.UserRole == "user");

            ViewBag.UserCount = userCount;
            var totalQuestionsCount = await _context.Questions
       .Where(q => selectedTopic.HasValue ? q.TopicId == selectedTopic.Value : true)
       .CountAsync();

            ViewBag.TotalQuestionsCount = totalQuestionsCount;

            return View(questions);
        }
        public IActionResult DownloadAllQuestions()
        {
            var allQuestions = _context.Questions.Include(q => q.Topic).ToList();

            // Create a CSV string
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Type,Question,Option1,Option2,Option3,Option4,Option5,CorrectOption,Topic,DifficultyLevel");

            foreach (var question in allQuestions)
            {
                csvContent.AppendLine($"{question.Type},{question.Question1},{question.Option1},{question.Option2},{question.Option3},{question.Option4},{question.Option5},{question.CorrectOption},{question.Topic.TopicName},{question.DifficultyLevel}");
            }

            // Return the CSV file as a response
            byte[] data = Encoding.UTF8.GetBytes(csvContent.ToString());
            return File(data, "text/csv", "AllQuestions.csv");
        }
        // GET: AddSingleQuestion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        // GET: AddSingleQuestion/Create
        public IActionResult Create()
        {
             
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName");
            return View();
        }
        // POST: AddSingleQuestion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Type,Question1,Option1,Option2,Option3,Option4,Option5,CorrectOption,TopicId,DifficultyLevel")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }           
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicId", question.TopicId);
            return View(question);
        }
        // GET: AddSingleQuestion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicId", question.TopicId);
            return View(question);
        }

        // POST: AddSingleQuestion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Type,Question1,Option1,Option2,Option3,Option4,Option5,CorrectOption,TopicId,DifficultyLevel")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
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
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicId", question.TopicId);
            return View(question);
        }
        // GET: AddSingleQuestion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        // POST: AddSingleQuestion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'ONLINEEXAMPORTALContext.Questions'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }         
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        private bool QuestionExists(int id)
        {
          return (_context.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
        }
       public IActionResult AddBulkQuestions()
        {
            return RedirectToAction("Index", "DataUpload2");
        }
        public IActionResult ManageUsers()
        {
            return RedirectToAction("Index", "Users");
        }
        public IActionResult Exampage()
        {
            return RedirectToAction("start", "quiz");
        }
        public IActionResult drplist()
        {
            return View();
        }
    }
}
