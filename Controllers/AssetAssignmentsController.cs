using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CorporateAssetManager.Data;
using CorporateAssetManager.Models;

namespace CorporateAssetManager.Controllers
{
    public class AssetAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetAssignments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AssetAssignments.Include(a => a.Asset).Include(a => a.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssetAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetAssignment = await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetAssignment == null)
            {
                return NotFound();
            }

            return View(assetAssignment);
        }

        // GET: AssetAssignments/Create
        public IActionResult Create()
        {
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: AssetAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,EmployeeId,AssignedDate,ReturnDate,Comments")] AssetAssignment assetAssignment)
        {
            // Verificar si el activo ya está asignado a otro empleado
            bool isAssetAvailable = await _context.AssetAssignments.AnyAsync(a => a.AssetId == assetAssignment.AssetId && a.ReturnDate == null);
            if (isAssetAvailable)
            {
                ModelState.AddModelError("AssetId", "El activo ya está asignado a otro empleado.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(assetAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", assetAssignment.AssetId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", assetAssignment.EmployeeId);
            return View(assetAssignment);
        }

        // GET: AssetAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetAssignment = await _context.AssetAssignments.FindAsync(id);
            if (assetAssignment == null)
            {
                return NotFound();
            }
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", assetAssignment.AssetId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", assetAssignment.EmployeeId);
            return View(assetAssignment);
        }

        // POST: AssetAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetId,EmployeeId,AssignedDate,ReturnDate,Comments")] AssetAssignment assetAssignment)
        {
            if (id != assetAssignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetAssignmentExists(assetAssignment.Id))
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
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", assetAssignment.AssetId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", assetAssignment.EmployeeId);
            return View(assetAssignment);
        }

        // GET: AssetAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetAssignment = await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetAssignment == null)
            {
                return NotFound();
            }

            return View(assetAssignment);
        }

        // POST: AssetAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetAssignment = await _context.AssetAssignments.FindAsync(id);
            if (assetAssignment != null)
            {
                _context.AssetAssignments.Remove(assetAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetAssignmentExists(int id)
        {
            return _context.AssetAssignments.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ReturnAsset(int? id){

            // Verificar si el id es nulo
            if (id == null)
            {
                return NotFound();
            }

            // Buscar el assetAssignment por el id
            var assetAssignment = await _context.AssetAssignments.FindAsync(id);
            if (assetAssignment == null)
            {
                return NotFound();
            }

            // Verificar si el assetAssignment ya tiene una fecha de devolución
            if (assetAssignment.ReturnDate != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Actualizar la fecha de devolución
            assetAssignment.ReturnDate = DateTime.Now;

            // Guardar los cambios
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
