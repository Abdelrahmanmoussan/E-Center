using IdentityText.Models;

namespace IdentityText.Repository.IRepository
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetfavoritesByUserIdAsync(string userId);
        Task AddTofavoritesAsync(string userId, int courseId);
        Task RemoveFromfavoritesAsync(string userId, int courseId);
    }

}
