using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Services;

public class ClienteApiClient
{
    private readonly HttpClient _http;

    public ClienteApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ClienteDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<ClienteDto>>("api/clientes") ?? new();

    public async Task<ClienteDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<ClienteDto>($"api/clientes/{id}");

    public async Task CreateAsync(ClienteCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/clientes", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, ClienteCreateDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/clientes/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/clientes/{id}");
        if (response.IsSuccessStatusCode) return (true, null);
        return (false, await response.Content.ReadAsStringAsync());
    }
}
