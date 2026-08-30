using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using Api_Clientes.Web.Models;

namespace Api_Clientes.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _http.GetFromJsonAsync<List<Cliente>>("api/clientes", JsonOptions);
            return View(clientes ?? new List<Cliente>());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _http.GetAsync($"api/clientes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>(JsonOptions);
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            var response = await _http.PostAsJsonAsync("api/clientes", cliente, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el cliente.");
                return View(cliente);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _http.GetAsync($"api/clientes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>(JsonOptions);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            if (!ModelState.IsValid) return View(cliente);

            var response = await _http.PutAsJsonAsync($"api/clientes/{id}", cliente, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el cliente.");
                return View(cliente);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _http.GetAsync($"api/clientes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>(JsonOptions);
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"api/clientes/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
