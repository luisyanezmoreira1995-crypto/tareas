using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using Api_Clientes.Web.Models;

namespace Api_Clientes.Web.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public ServiciosController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            var servicios = await _http.GetFromJsonAsync<List<Servicio>>("api/servicios", JsonOptions);
            return View(servicios ?? new List<Servicio>());
        }

        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _http.GetAsync($"api/servicios/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var servicio = await response.Content.ReadFromJsonAsync<Servicio>(JsonOptions);
            return View(servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (!ModelState.IsValid) return View(servicio);

            var response = await _http.PostAsJsonAsync("api/servicios", servicio, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el servicio.");
                return View(servicio);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _http.GetAsync($"api/servicios/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var servicio = await response.Content.ReadFromJsonAsync<Servicio>(JsonOptions);
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servicio servicio)
        {
            if (id != servicio.Id) return BadRequest();
            if (!ModelState.IsValid) return View(servicio);

            var response = await _http.PutAsJsonAsync($"api/servicios/{id}", servicio, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el servicio.");
                return View(servicio);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _http.GetAsync($"api/servicios/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var servicio = await response.Content.ReadFromJsonAsync<Servicio>(JsonOptions);
            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"api/servicios/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
