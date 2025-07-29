namespace CompanyManagementAPI.Services;

public class RandomStringGeneratorService : IRandomStringGeneratorService
{
    private readonly HttpClient _httpClient;
    public RandomStringGeneratorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GenerateCodeAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                "https://www.random.org/strings/?num=1&len=8&digits=on&upperalpha=on&loweralpha=off&unique=on&format=plain&rnd=new");
            return response.Trim();
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException("Failed to generate project code", ex);
        }
    }
}