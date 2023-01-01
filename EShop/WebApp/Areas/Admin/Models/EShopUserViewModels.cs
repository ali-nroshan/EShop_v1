using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models
{

	public class EShopUserCreateOrEditViewModel
	{
		[Required]
		public int UserId { get; set; }

		[Required, EmailAddress, MaxLength(100)]
		public string Email { get; set; } = null!;

		[Required, DataType(DataType.Password), MaxLength(50)]
		public string Password { get; set; } = null!;

		[Required]
		public bool IsAdmin { get; set; }
	}
}
