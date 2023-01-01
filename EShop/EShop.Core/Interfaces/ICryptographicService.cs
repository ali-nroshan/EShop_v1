namespace EShop.Core.Interfaces
{
    public interface ICryptographicService
    {
        string ComputeSHA512(string data, string salt);
    }
}
