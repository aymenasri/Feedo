using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using Feedo.Entities;
using Feedo.Shared;

namespace Feedo.Controllers
{
    public class LivreursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivreursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Livreurs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Livreurs.ToListAsync());
        }

        // GET: Livreurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var livreur = await _context.Livreurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livreur == null) return NotFound();

            return View(livreur);
        }

        // GET: Livreurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Livreurs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleType,LicensePlate,Nom,Prenom,Email,NumeroTelephone")] Livreur livreur)
        {
            if (ModelState.IsValid)
            {
                livreur.CreationA = DateTime.Now;
                livreur.Status = LivreurStatus.Offline; // Default status
                livreur.Compte = livreur.Email; // Temp auth logic
                livreur.MotPasse = "123456"; // Default password for demo
                
                _context.Add(livreur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(livreur);
        }

        // GET: Livreurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var livreur = await _context.Livreurs.FindAsync(id);
            if (livreur == null) return NotFound();
            return View(livreur);
        }

        // POST: Livreurs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,LicensePlate,Status,Nom,Prenom,Email,NumeroTelephone,Compte,MotPasse,CreationA")] Livreur livreur)
        {
            if (id != livreur.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livreur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivreurExists(livreur.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livreur);
        }

        // GET: Livreurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var livreur = await _context.Livreurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livreur == null) return NotFound();

            return View(livreur);
        }

        // POST: Livreurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livreur = await _context.Livreurs.FindAsync(id);
            if (livreur != null)
            {
                _context.Livreurs.Remove(livreur);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Livreurs/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var livreur = await _context.Livreurs.FindAsync(id);
            if (livreur == null) return Json(new { success = false });

            // Toggle logic: Offline -> Available -> Offline (Busy is handled by OrderService)
            if (livreur.Status == LivreurStatus.Offline)
            {
                livreur.Status = LivreurStatus.Available;
            }
            else if (livreur.Status == LivreurStatus.Available)
            {
                livreur.Status = LivreurStatus.Offline;
            }
            // If Busy, we typically strictly don't allow manual toggle unless admin force override, 
            // but for simplicity let's allow forcing Offline.
            else if (livreur.Status == LivreurStatus.Busy)
            {
                livreur.Status = LivreurStatus.Offline;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, newStatus = livreur.Status.ToString() });
        }

        private bool LivreurExists(int id)
        {
            return _context.Livreurs.Any(e => e.Id == id);
        }
    }
}
