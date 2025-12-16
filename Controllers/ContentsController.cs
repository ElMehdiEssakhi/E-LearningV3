using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_LearningV3;
using E_LearningV3.Models;

namespace E_LearningV3.Controllers
{
    public class ContentsController : Controller
    {
        private readonly AppDbContext _context;

        public ContentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Contents
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Contents.Include(c => c.Chapter);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Contents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(m => m.ContentId == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Contents/Create
        public IActionResult Create(int chapId)
        {
            var content = new Content
            {
                ChapId = chapId // set the ChapId here
            };
            ViewData["TypeList"] = new SelectList(Enum.GetValues(typeof(Content.ContentType)));

            return View(content);
        }

        // POST: Contents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContentId,Type,ContentLink,ChapId")] Content content)
        {
            if (ModelState.IsValid)
            {
                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Chapters", new { id = content.ChapId });
            }
            ViewData["TypeList"] = new SelectList(Enum.GetValues(typeof(Content.ContentType)));
            return View(content);
        }

        // GET: Contents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            ViewData["TypeList"] = new SelectList(Enum.GetValues(typeof(Content.ContentType)), content.Type);
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentId,Type,ContentLink,ChapId")] Content content)
        {
            if (id != content.ContentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.ContentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Chapters", new { id = content.ChapId });
            }
            ViewData["ChapId"] = new SelectList(
       _context.Chapters.Where(c => c.ChapId == content.ChapId),
       "ChapId", "Title", content.ChapId);

            ViewData["TypeList"] = new SelectList(Enum.GetValues(typeof(Content.ContentType)), content.Type);

            return View(content);
        }

        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(m => m.ContentId == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                _context.Contents.Remove(content);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Chapters", new { id = content.ChapId });
        }

        private bool ContentExists(int id)
        {
            return _context.Contents.Any(e => e.ContentId == id);
        }
    }
}
