namespace Company_ManagementAPI.Services;

public class RandomStringGeneratorService : IRandomStringGenerator
{
    private readonly HttpClient _http = new();
    public async Task<string> GenerateAsync()
    {
        var response = await _http.GetStringAsync("https://www.random.org/strings/?num=1&len=8&digits=on&upperalpha=on&loweralpha=off&unique=on&format=plain&rnd=new");
        return response.Trim();
    }
}