using EShop.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Infrastructure.Common
{
	public class CryptographicService : ICryptographicService
	{
		public string ByteToBase64(byte[] data) => Convert.ToBase64String(data);


		public string ComputeSHA512(string data, string salt)
		{
			using (SHA512 sha512 = SHA512.Create())
			{
				var rt = sha512.ComputeHash(Encoding.UTF8.GetBytes(data + salt));
				return ByteToBase64(rt);
			}
		}
	}
}
