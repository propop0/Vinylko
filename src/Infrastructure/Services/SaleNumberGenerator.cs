using System.Text;

namespace Infrastructure.Services;

public interface ISaleNumberGenerator
{
    string GenerateSaleNumber();
}

public class SaleNumberGenerator : ISaleNumberGenerator
{
    public string GenerateSaleNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"SALE-{timestamp}-{random}";
    }
}
