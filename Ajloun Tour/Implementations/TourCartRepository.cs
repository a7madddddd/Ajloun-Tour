using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.DTOs2.TourCartDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class TourCartRepository : ITourCartRepository
    {
        private readonly MyDbContext _context;

        public TourCartRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourCartDTO>> GetCarts()
        {
            return await _context.Carts
                        .Include(c => c.User)
                        .Select(c => new TourCartDTO
                        {
                            CartID = c.CartId,
                            UserID = c.UserId,
                            CreatedAt = (DateTime)c.CreatedAt,
                            UpdatedAt = (DateTime)c.UpdatedAt,
                            Status = c.Status,
                            User = new UsersDTO
                            {
                                UserId = c.User.UserId,
                                FullName = c.User.FullName,
                                Email = c.User.Email
                            }
                        })
                        .ToListAsync();
        }
        public async Task<TourCartDTO> GetCartById(int cartId)
        {
            var cart = await _context.Carts
                        .Where(c => c.CartId == cartId)
                        .Include(c => c.User)
                        .FirstOrDefaultAsync();

            if (cart == null)
            {
                return null;
            }

            return new TourCartDTO
            {
                CartID = cart.CartId,
                UserID = cart.UserId,
                CreatedAt = (DateTime)cart.CreatedAt,
                UpdatedAt = (DateTime)cart.UpdatedAt,
                Status = cart.Status,
                User = new UsersDTO
                {
                    UserId = cart.User.UserId,
                    FullName = cart.User.FullName,
                    Email = cart.User.Email
                }
            };
        }
        public async Task<TourCartDTO> AddCart(CreateTourCart createTourCart)
        {
            var cart = new Cart
            {
                UserId = createTourCart.UserID,
                Status = createTourCart.Status
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return new TourCartDTO
            {
                CartID = cart.CartId,
                UserID = cart.UserId,
                CreatedAt = (DateTime)cart.CreatedAt,
                UpdatedAt = (DateTime)cart.UpdatedAt,
                Status = cart.Status
            };
        }

        public async Task<TourCartDTO> UpdateCart(int cartId, CreateTourCart updateCartDTO)
        {
            var cart = await _context.Carts.FindAsync(cartId);

            if (cart == null)
            {
                return null;
            }

            cart.Status = updateCartDTO.Status;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new TourCartDTO
            {
                CartID = cart.CartId,
                UserID = cart.UserId,
                CreatedAt = (DateTime)cart.CreatedAt,
                UpdatedAt = (DateTime)cart.UpdatedAt,
                Status = cart.Status
            };
        }
        public async Task DeleteCart(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);

            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

    }
}
