using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sway.Data;
using Sway.Models;

namespace Sway.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly SwayContext _context;

        public AssessmentsController(SwayContext context)
        {
            _context = context;
        }

        // GET: Assessments
        public async Task<IActionResult> Index()
        {
            var swayContext = _context.Assessment.Include(a => a.Opinion);
            return View(await swayContext.ToListAsync());
        }

        // GET: Assessments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment
                .Include(a => a.Opinion)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // GET: Assessments/Create
        public IActionResult Create()
        {
            ViewData["OpinionID"] = new SelectList(_context.Opinion, "ID", "oName");
            return View();
        }

        // POST: Assessments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,aName,aSentiment,aPosSentiment,aNegSentiment,OpinionID")] Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OpinionID"] = new SelectList(_context.Opinion, "ID", "oName", assessment.OpinionID);
            return View(assessment);
        }

        // GET: Assessments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment.FindAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["OpinionID"] = new SelectList(_context.Opinion, "ID", "oName", assessment.OpinionID);
            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,aName,aSentiment,aPosSentiment,aNegSentiment,OpinionID")] Assessment assessment)
        {
            if (id != assessment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentExists(assessment.ID))
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
            ViewData["OpinionID"] = new SelectList(_context.Opinion, "ID", "oName", assessment.OpinionID);
            return View(assessment);
        }

        // GET: Assessments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment
                .Include(a => a.Opinion)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assessment == null)
            {
                return Problem("Entity set 'SwayContext.Assessment'  is null.");
            }
            var assessment = await _context.Assessment.FindAsync(id);
            if (assessment != null)
            {
                _context.Assessment.Remove(assessment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentExists(int id)
        {
          return _context.Assessment.Any(e => e.ID == id);
        }
    }
}
