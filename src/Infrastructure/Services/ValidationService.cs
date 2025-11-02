using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public interface IValidationService
{
    bool IsValidEmail(string email);
    bool IsValidWebsite(string website);
    bool IsValidPrice(decimal price);
    bool IsValidReleaseYear(int year);
}

public class ValidationService : IValidationService
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex WebsiteRegex = new(@"^https?://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled);

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex.IsMatch(email);
    }

    public bool IsValidWebsite(string website)
    {
        if (string.IsNullOrWhiteSpace(website))
            return true; 

        return WebsiteRegex.IsMatch(website);
    }

    public bool IsValidPrice(decimal price)
    {
        return price > 0 && price <= 999999.99m; 
    }

    public bool IsValidReleaseYear(int year)
    {
        var currentYear = DateTime.UtcNow.Year;
        return year >= 1900 && year <= currentYear + 1;
    }
}
