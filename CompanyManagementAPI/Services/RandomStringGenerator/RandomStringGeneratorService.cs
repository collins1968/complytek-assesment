using CompanyManagementAPI.Domain;
using CompanyManagementAPI.Extensions;

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
            var response = await _httpClient.GetStringAsync("https://codito.io/free-random-code-generator/api/generate");
            return response.Trim();
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException(AppErrors.GenerateProjectFail.Description(), ex);
        }
    }
}