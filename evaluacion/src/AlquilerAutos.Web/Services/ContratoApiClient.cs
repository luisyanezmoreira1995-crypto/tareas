using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Services;

public class ContratoApiClient
{
    private readonly HttpClient _http;

    public ContratoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ContratoDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<ContratoDto>>("api/contratos") ?? new();

    public async Task<ContratoDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<ContratoDto>($"api/contratos/{id}");

    public async Task<(bool Ok, string? Error)> CreateAsync(ContratoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/contratos", dto);
        if (response.IsSuccessStatusCode) return (true, null);
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool Ok, string? Error)> UpdateAsync(int id, ContratoUpdateDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/contratos/{id}", dto);
        if (response.IsSuccessStatusCode) return (true, null);
        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/contratos/{id}");
        response.EnsureSuccessStatusCode();
    }
}
