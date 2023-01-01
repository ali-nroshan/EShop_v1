using EShop.Domain.Entities;

namespace EShop.Domain.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(EShopUser user);

        EShopUser? GetUser(string email, string password);

        EShopUser? GetUserByEmail(string email);

        EShopUser? GetUserById(int userId);

        IQueryable<EShopUser> GetEShopUsers();

        void RemoveUser(EShopUser user);

        void UpdateUser(EShopUser user);


        void GetUserSuccessfulOrderCountAndTotalPrice(int userId, ref int successfulOrdersCount,ref decimal totalPrice);

	}
}
