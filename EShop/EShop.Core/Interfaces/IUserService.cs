using EShop.Core.ViewModels;
using EShop.Domain.Entities;

namespace EShop.Core.Interfaces
{
	public interface IUserService
	{
		public EShopUser? GetUser(string email, string password);
		public bool CheckUserCredentials(string email, string password);
		public bool EmailExist(string email);
		public void AddUser(string email, string password, bool admin = false);
		public EShopUserViewModel? GetUserById(int userId);

		public void UpdateUserCredentials(int userId, string password, string email, bool admin);
		public void DeleteUser(int userId);

		//public EShopUserReportViewModel GetUserReport(int userId);
		public List<EShopUserReportViewModel> GetUserReports();
		#region UserWishList
		public List<ProductItemViewModel> GetUserWishList(int userId);

		bool AddProductToWishList(int userId, int productId);

		bool RemoveProductFromWishList(int userId, int productId);

		int GetWishListProductCount(int userId);

		#endregion
	}
}
