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
    public class PhrasesController : Controller
    {
        private readonly SwayContext _context;

        public PhrasesController(SwayContext context)
        {
            _context = context;
        }

        // GET: Phrases
        public async Task<IActionResult> Index()
        {
            var swayContext = _context.Phrases.Include(p => p.Document);
            return View(await swayContext.ToListAsync());
        }

        // GET: Phrases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phrases == null)
            {
                return NotFound();
            }

            var phrase = await _context.Phrases
                .Include(p => p.Document)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (phrase == null)
            {
                return NotFound();
            }

            return View(phrase);
        }

        // GET: Phrases/Create
        public IActionResult Create()
        {
            ViewData["DocumentID"] = new SelectList(_context.Documents, "ID", "dName");
            return View();
        }

        // POST: Phrases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,pName,sentiment,posSentiment,negSentiment,neutralSentiment,DocumentID")] Phrase phrase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phrase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentID"] = new SelectList(_context.Documents, "ID", "dName", phrase.DocumentID);
            return View(phrase);
        }

        // GET: Phrases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Phrases == null)
            {
                return NotFound();
            }

            var phrase = await _context.Phrases.FindAsync(id);
            if (phrase == null)
            {
                return NotFound();
            }
            ViewData["DocumentID"] = new SelectList(_context.Documents, "ID", "dName", phrase.DocumentID);
            return View(phrase);
        }

        // POST: Phrases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,pName,sentiment,posSentiment,negSentiment,neutralSentiment,DocumentID")] Phrase phrase)
        {
            if (id != phrase.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phrase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhraseExists(phrase.ID))
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
            ViewData["DocumentID"] = new SelectList(_context.Documents, "ID", "dName", phrase.DocumentID);
            return View(phrase);
        }

        // GET: Phrases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Phrases == null)
            {
                return NotFound();
            }

            var phrase = await _context.Phrases
                .Include(p => p.Document)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (phrase == null)
            {
                return NotFound();
            }

            return View(phrase);
        }

        // POST: Phrases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Phrases == null)
            {
                return Problem("Entity set 'SwayContext.Phrases'  is null.");
            }
            var phrase = await _context.Phrases.FindAsync(id);
            if (phrase != null)
            {
                _context.Phrases.Remove(phrase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhraseExists(int id)
        {
          return _context.Phrases.Any(e => e.ID == id);
        }
    }
}
