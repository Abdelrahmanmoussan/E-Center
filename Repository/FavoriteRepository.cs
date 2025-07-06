using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favorite>> GetfavoritesByUserIdAsync(string userId)
        {
            return await _context.favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.ClassGroup)
                .ToListAsync();
        }

        public async Task AddTofavoritesAsync(string userId, int ClassGroupId)
        {
            var exists = await _context.favorites.AnyAsync(f => f.UserId == userId && f.ClassGroupId == ClassGroupId);
            if (!exists)
            {
                _context.favorites.Add(new Favorite { UserId = userId, ClassGroupId = ClassGroupId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromfavoritesAsync(string userId, int ClassGroupId)
        {
            var fav = await _context.favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ClassGroupId == ClassGroupId);
            if (fav != null)
            {
                _context.favorites.Remove(fav);
                await _context.SaveChangesAsync();
            }
        }
    }

}
