using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Services;

public class ReporteApiClient
{
    private readonly HttpClient _http;

    public ReporteApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ReporteContratosClienteDto?> ContratosPorClienteAsync(int clienteId) =>
        await _http.GetFromJsonAsync<ReporteContratosClienteDto>($"api/reportes/contratos-por-cliente/{clienteId}");
}
