using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_Rukaiya.Data;
using StudentManagementSystem_Rukaiya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem_Rukaiya.Controllers
{
    //[Authorize]
    public class GradingRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradingRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GradingRules
        public async Task<IActionResult> Index()
        {
            return View(await _context.GradingRules.ToListAsync());
        }

        // GET: GradingRules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradingRule = await _context.GradingRules
                .FirstOrDefaultAsync(m => m.GradingRuleId == id);
            if (gradingRule == null)
            {
                return NotFound();
            }

            return View(gradingRule);
        }

        // GET: GradingRules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GradingRules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradingRuleId,Grade,MinMarks,MaxMarks,Remarks")] GradingRule gradingRule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradingRule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gradingRule);
        }

        // GET: GradingRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradingRule = await _context.GradingRules.FindAsync(id);
            if (gradingRule == null)
            {
                return NotFound();
            }
            return View(gradingRule);
        }

        // POST: GradingRules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradingRuleId,Grade,MinMarks,MaxMarks,Remarks")] GradingRule gradingRule)
        {
            if (id != gradingRule.GradingRuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradingRule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradingRuleExists(gradingRule.GradingRuleId))
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
            return View(gradingRule);
        }

        // GET: GradingRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradingRule = await _context.GradingRules
                .FirstOrDefaultAsync(m => m.GradingRuleId == id);
            if (gradingRule == null)
            {
                return NotFound();
            }

            return View(gradingRule);
        }

        // POST: GradingRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gradingRule = await _context.GradingRules.FindAsync(id);
            if (gradingRule != null)
            {
                _context.GradingRules.Remove(gradingRule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradingRuleExists(int id)
        {
            return _context.GradingRules.Any(e => e.GradingRuleId == id);
        }
    }
}
