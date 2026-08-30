using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using Api_Clientes.Web.Models;

namespace Api_Clientes.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _http.GetFromJsonAsync<List<Empleado>>("api/empleados", JsonOptions);
            return View(empleados ?? new List<Empleado>());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _http.GetAsync($"api/empleados/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>(JsonOptions);
            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View(new Empleado { FechaContratacion = DateTime.Today });
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            var response = await _http.PostAsJsonAsync("api/empleados", empleado, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el empleado.");
                return View(empleado);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _http.GetAsync($"api/empleados/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>(JsonOptions);
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id) return BadRequest();
            if (!ModelState.IsValid) return View(empleado);

            var response = await _http.PutAsJsonAsync($"api/empleados/{id}", empleado, JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el empleado.");
                return View(empleado);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _http.GetAsync($"api/empleados/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>(JsonOptions);
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"api/empleados/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
