using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Services;

public class VehiculoApiClient
{
    private readonly HttpClient _http;

    public VehiculoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<VehiculoDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<VehiculoDto>>("api/vehiculos") ?? new();

    public async Task<VehiculoDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<VehiculoDto>($"api/vehiculos/{id}");

    public async Task CreateAsync(VehiculoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/vehiculos", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, VehiculoCreateDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/vehiculos/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/vehiculos/{id}");
        if (response.IsSuccessStatusCode) return (true, null);
        return (false, await response.Content.ReadAsStringAsync());
    }
}
