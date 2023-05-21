using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sway.Data;
using Sway.Models;
using System.Linq;

namespace Sway.Controllers
{
    public class OpinionsController : Controller
    {
        private readonly SwayContext _context;

        public OpinionsController(SwayContext context)
        {
            _context = context;
        }

        // GET: Opinions
        public async Task<IActionResult> Index()
        {
            var swayContext = _context.Opinion.Include(o => o.Phrase);
            return View(await swayContext.ToListAsync());
        }

        // GET: Opinions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Opinion == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion
                .Include(o => o.Phrase)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // GET: Opinions/Create
        public IActionResult Create()
        {
            ViewData["PhraseID"] = new SelectList(_context.Phrases, "ID", "pName");
            return View();
        }

        // POST: Opinions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,oName,oSentiment,oPosSentiment,oNegSentiment,PhraseID")] Opinion opinion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opinion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhraseID"] = new SelectList(_context.Phrases, "ID", "pName", opinion.PhraseID);
            return View(opinion);
        }

        // GET: Opinions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Opinion == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion.FindAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }
            ViewData["PhraseID"] = new SelectList(_context.Phrases, "ID", "pName", opinion.PhraseID);
            return View(opinion);
        }

        // POST: Opinions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,oName,oSentiment,oPosSentiment,oNegSentiment,PhraseID")] Opinion opinion)
        {
            if (id != opinion.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opinion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpinionExists(opinion.ID))
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
            ViewData["PhraseID"] = new SelectList(_context.Phrases, "ID", "pName", opinion.PhraseID);
            return View(opinion);
        }

        // GET: Opinions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Opinion == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion
                .Include(o => o.Phrase)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // POST: Opinions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Opinion == null)
            {
                return Problem("Entity set 'SwayContext.Opinion'  is null.");
            }
            var opinion = await _context.Opinion.FindAsync(id);
            if (opinion != null)
            {
                _context.Opinion.Remove(opinion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpinionExists(int id)
        {
          return _context.Opinion.Any(e => e.ID == id);
        }
    }
}
