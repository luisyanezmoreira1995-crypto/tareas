namespace API_TAREA2.Services;

public class NoticiasService : INoticiasService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public NoticiasService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("API_TAREA2/1.0");
        _config = config;
    }

    public async Task<string> ObtenerTitularesAsync()
    {
        var apiKey = _config["NewsApi:ApiKey"];

        var response = await _httpClient.GetAsync(
            $"https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey={apiKey}");

        return await response.Content.ReadAsStringAsync();
    }
}
