using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Services;

public class PagoApiClient
{
    private readonly HttpClient _http;

    public PagoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<PagoDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<PagoDto>>("api/pagos") ?? new();

    public async Task<PagoDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<PagoDto>($"api/pagos/{id}");

    public async Task<(bool Ok, string? Error)> CreateAsync(PagoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/pagos", dto);
        if (response.IsSuccessStatusCode) return (true, null);
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/pagos/{id}");
        response.EnsureSuccessStatusCode();
    }
}
