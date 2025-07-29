namespace Company_ManagementAPI.Services;

public interface IRandomStringGeneratorService
{
    Task<string> GenerateCodeAsync();
}