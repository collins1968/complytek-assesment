using System.ComponentModel;

namespace CompanyManagementAPI.Domain;

public enum AppErrors
{
    [Description("Failed to generate project code")]
    GenerateProjectFail = 101,
}