namespace Company_ManagementAPI.Services;

public interface IRandomStringGenerator
{
    Task<string> GenerateAsync();
}