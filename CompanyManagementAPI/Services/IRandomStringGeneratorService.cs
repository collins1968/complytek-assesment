namespace CompanyManagementAPI.Services;

public interface IRandomStringGeneratorService
{
    Task<string> GenerateCodeAsync();
}