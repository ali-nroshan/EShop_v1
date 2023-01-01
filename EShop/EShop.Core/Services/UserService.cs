using EShop.Core.Common;
using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Entities;
using EShop.Domain.Interfaces;

namespace EShop.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserWishListRepository wishListRepository;
        private readonly ICryptographicService cryptographicService;

        public UserService(IUserRepository userRepository,
            IUserWishListRepository wishListRepository,
            ICryptographicService cryptographicService)
        {
            this.userRepository = userRepository;
            this.wishListRepository = wishListRepository;
            this.cryptographicService = cryptographicService;
        }

		#region Admin
		public EShopUserViewModel? GetUserById(int userId)
		{
            var user = userRepository.GetUserById(userId);
            return (user != null)?
                new EShopUserViewModel()
                {
                    RegisterDate = user.RegisterDate.GregorianToPersianDate(),
                    UserEmail = user.Email,
                    UserRole = user.IsAdmin?"Admin":"User",
                    UserId = userId
                } : null;
		}

		public void UpdateUserCredentials(int userId, string password, string email, bool admin)
		{
            var user = userRepository.GetUserById(userId);
            if (user == null) throw new Exception("user not found 404");
            user.Password = cryptographicService.ComputeSHA512(password, GetPasswordSalt(password));
            user.Email = email;
            user.IsAdmin= admin;
            userRepository.UpdateUser(user);
		}

		public void DeleteUser(int userId)
		{
			var user = userRepository.GetUserById(userId);
			if (user == null) throw new Exception("user not found 404");
            userRepository.RemoveUser(user);
		}

		private EShopUserReportViewModel GetUserReport(EShopUser user)
		{
            var user_report = new EShopUserReportViewModel()
            {
                Password = "123",
                RegisterDate = user.RegisterDate.GregorianToPersianDate(),
                UserEmail = user.Email,
                UserId = user.Id,
                UserRole = user.IsAdmin ? "Admin" : "User"
            };
            decimal totalPurchase = 0;
            int totalOrder = 0;
            userRepository.GetUserSuccessfulOrderCountAndTotalPrice(user.Id, ref totalOrder, ref totalPurchase);
            user_report.SuccessfulOrdersCount = totalOrder;
            user_report.SuccessfulOrdersPriceTotal = totalPurchase;
            return user_report;
		}

		public List<EShopUserReportViewModel> GetUserReports()
		{
            return userRepository.GetEShopUsers().ToList()
				.Select(q => GetUserReport(q)).ToList();
		}

		#endregion
		#region User
		private string GetRawEmail(string email)
        {
            return email.Trim().ToLower();
        }

        private bool CheckPasswordStrength(string password)
        {
            if (password.Length < 8)
                return false;
            return true;
        }

        private string GetPasswordSalt(string password)
        {
            return password.TakeLast(password.Length / 2).ToString()!;
        }


        public void AddUser(string email, string password, bool admin = false)
        {
            var user = userRepository.GetUserByEmail(GetRawEmail(email));
            if (user != null) throw new Exception("User aleady exist");
            if (!CheckPasswordStrength(password)) throw new Exception("Password is weak");
            userRepository.AddUser(new EShopUser()
            {
                Email = GetRawEmail(email),
                Password = cryptographicService
                .ComputeSHA512(password, GetPasswordSalt(password)),
                IsAdmin = admin,
                RegisterDate = DateTime.UtcNow
            });
        }

        public bool CheckUserCredentials(string email, string password)
        {
            var user = userRepository.GetUserByEmail(GetRawEmail(email));
            if (user == null) return false;
            if (user.Password == password) return true;
            return false;
        }

        public EShopUser? GetUser(string email, string password)
        {
            var user = userRepository.GetUserByEmail(GetRawEmail(email));
            if (user is null ||
                user.Password != cryptographicService
                .ComputeSHA512(password, GetPasswordSalt(password)))
                return null;
            return new EShopUser()
            {
                Email = user.Email,
                Id = user.Id,
                IsAdmin = user.IsAdmin,
                RegisterDate = user.RegisterDate
            };
        }

        public bool EmailExist(string email)
        {
            return (userRepository.GetUserByEmail(GetRawEmail(email)) == null) ? false : true;
        }


        #endregion
        #region WishList
        public bool AddProductToWishList(int userId, int productId)
        {
            return wishListRepository.AddProductToWishList(userId, productId);
        }

        public List<ProductItemViewModel> GetUserWishList(int userId)
        {
            return wishListRepository.GetUserWishList(userId)
                .Select(q => new ProductItemViewModel()
                {
                    HasInventory = (q.QuantityInStock > 0) ? true : false,
                    ProductDescription = q.ProductDescription,
                    ProductName = q.ProductName,
                    ProductPrice = q.ProductPrice,
                    ProductId = q.ProductId,
                    ProductImageFile = q.ProductImageFileName,
                    ProductScore = q.ProductScore
                }).ToList();
        }

        public int GetWishListProductCount(int userId)
        {
            return wishListRepository.GetWishListProductCount(userId);
        }

        public bool RemoveProductFromWishList(int userId, int productId)
        {
            return wishListRepository.RemoveProductFromWishList(userId, productId);
        }


		#endregion
	}
}
