using API_TAREA2.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_TAREA2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoticiasController : ControllerBase
{
    private readonly INoticiasService _noticiasService;

    public NoticiasController(INoticiasService noticiasService)
    {
        _noticiasService = noticiasService;
    }

    [HttpGet]
    public async Task<ContentResult> Get()
    {
        var contenido = await _noticiasService.ObtenerTitularesAsync();
        return Content(contenido, "application/json");
    }
}
