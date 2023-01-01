using System.ComponentModel.DataAnnotations;

namespace EShop.Core.ViewModels
{
	public class EShopUserViewModel
	{
		public int UserId { get; set; }
		public string UserEmail { get; set; } = null!;
		public string Password { get { return "123"; } }
		public string RegisterDate { get; set; } = null!;
		public string UserRole { get; set; } = null!;
	}

	public class EShopUserReportViewModel
	{
		public int UserId { get; set; }

		[EmailAddress]
		public string UserEmail { get; set; } = null!;

		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		public string RegisterDate { get; set; } = null!;

		public int SuccessfulOrdersCount { get; set; }

		public decimal SuccessfulOrdersPriceTotal { get; set; }

		public string UserRole { get; set; } = "User";
	}


}
