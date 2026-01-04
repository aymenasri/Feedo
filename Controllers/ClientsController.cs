using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Feedo.Entities;
using Feedo.Services;

namespace Feedo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // --- LECTURE (READ) ---
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return View(clients);
        }

        // --- CRÉATION (CREATE) ---
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken] // Protection contre les attaques Cross-Site Request Forgery
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clientService.CreateClientAsync(client);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException)
                {
                    ModelState.AddModelError("", "Invalid client data.");
                }
            }
            return View(client);
        }

        // --- MODIFICATION (UPDATE) ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var client = await _clientService.GetClientByIdAsync(id.Value);
                return View(client);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _clientService.UpdateClientAsync(client);
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (ArgumentNullException)
                {
                    ModelState.AddModelError("", "Invalid client data.");
                }
            }
            return View(client);
        }

        // --- SUPPRESSION (DELETE) ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var client = await _clientService.GetClientByIdAsync(id.Value);
                return View(client);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _clientService.DeleteClientAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}