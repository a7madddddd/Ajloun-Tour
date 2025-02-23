using Ajloun_Tour.DTOs.AdminsDTOs;
using Ajloun_Tour.DTOs.UsersDTOs;
using Ajloun_Tour.DTOs2.TourCartDTOs;
using Ajloun_Tour.DTOs2.TourCartItemsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class TourCartItemsRepository : ITourCartItemsRepository
    {
        private readonly MyDbContext _context;

        public TourCartItemsRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourCartItemsDTO>> GetCartItems()
        {
            return await _context.CartItems
                   .Include(ci => ci.Cart)
                   .ThenInclude(c => c.User)
                   .Select(ci => new TourCartItemsDTO
                   {
                       CartItemId = ci.CartItemId,
                       CartId = ci.CartId,
                       TourId = ci.TourId,
                       Quantity = ci.Quantity ?? 1,
                       Price = ci.Price,
                       SelectedDate = ci.SelectedDate,
                       NumberOfPeople = ci.NumberOfPeople,
                       HasTourGuide = ci.HasTourGuide ?? false,
                       HasInsurance = ci.HasInsurance ?? false,
                       HasDinner = ci.HasDinner ?? false,
                       HasBikeRent = ci.HasBikeRent ?? false,
                       TourGuidePrice = ci.TourGuidePrice ?? 0,
                       InsurancePrice = ci.InsurancePrice ?? 0,
                       DinnerPrice = ci.DinnerPrice ?? 0,
                       BikeRentPrice = ci.BikeRentPrice ?? 0,
                       CreatedAt = ci.CreatedAt ?? DateTime.UtcNow,
                       UpdatedAt = ci.UpdatedAt ?? DateTime.UtcNow,

                       // Cart Details
                       UserID = ci.Cart.UserId,
                       Status = ci.Cart.Status ?? "active",

                       // User Details
                       User = new UsersDTO
                       {
                           UserId = ci.Cart.User.UserId,
                           FullName = ci.Cart.User.FullName,
                           Email = ci.Cart.User.Email
                       }
                   })
                   .ToListAsync();
        }

        public async Task<TourCartItemsDTO> GetCartItemsByCartId(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");

            return new TourCartItemsDTO
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                TourId = cartItem.TourId,
                Quantity = cartItem.Quantity ?? 1,
                Price = cartItem.Price,
                SelectedDate = cartItem.SelectedDate,
                NumberOfPeople = cartItem.NumberOfPeople,
                HasTourGuide = cartItem.HasTourGuide ?? false,
                HasInsurance = cartItem.HasInsurance ?? false,
                HasDinner = cartItem.HasDinner ?? false,
                HasBikeRent = cartItem.HasBikeRent ?? false,
                TourGuidePrice = cartItem.TourGuidePrice ?? 0,
                InsurancePrice = cartItem.InsurancePrice ?? 0,
                DinnerPrice = cartItem.DinnerPrice ?? 0,
                BikeRentPrice = cartItem.BikeRentPrice ?? 0,
                CreatedAt = cartItem.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = cartItem.UpdatedAt ?? DateTime.UtcNow,

                // Cart and User Details
                UserID = cartItem.Cart.UserId,
                Status = cartItem.Cart.Status ?? "active",
                User = new UsersDTO
                {
                    UserId = cartItem.Cart.User.UserId,
                    FullName = cartItem.Cart.User.FullName,
                    Email = cartItem.Cart.User.Email
                }
            };
        }

        public async Task<TourCartItemsDTO> AddCartItem(CreateCartItemDTO createTourCart)
        {
            var cartItem = new CartItem
            {
                CartId = createTourCart.CartID,
                TourId = createTourCart.TourID,
                Quantity = createTourCart.Quantity,
                Price = createTourCart.Price,
                SelectedDate = createTourCart.SelectedDate,
                NumberOfPeople = createTourCart.NumberOfPeople,
                HasTourGuide = createTourCart.HasTourGuide,
                HasInsurance = createTourCart.HasInsurance,
                HasDinner = createTourCart.HasDinner,
                HasBikeRent = createTourCart.HasBikeRent,
                TourGuidePrice = createTourCart.TourGuidePrice,
                InsurancePrice = createTourCart.InsurancePrice,
                DinnerPrice = createTourCart.DinnerPrice,
                BikeRentPrice = createTourCart.BikeRentPrice,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return await GetCartItemsByCartId(cartItem.CartItemId);
        }

        public async Task<TourCartItemsDTO> UpdateCartItem(int cartItemId, CreateCartItemDTO updateCartItemDTO)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");

            cartItem.TourId = updateCartItemDTO.TourID;
            cartItem.Quantity = updateCartItemDTO.Quantity;
            cartItem.Price = updateCartItemDTO.Price;
            cartItem.SelectedDate = updateCartItemDTO.SelectedDate;
            cartItem.NumberOfPeople = updateCartItemDTO.NumberOfPeople;
            cartItem.HasTourGuide = updateCartItemDTO.HasTourGuide;
            cartItem.HasInsurance = updateCartItemDTO.HasInsurance;
            cartItem.HasDinner = updateCartItemDTO.HasDinner;
            cartItem.HasBikeRent = updateCartItemDTO.HasBikeRent;
            cartItem.TourGuidePrice = updateCartItemDTO.TourGuidePrice;
            cartItem.InsurancePrice = updateCartItemDTO.InsurancePrice;
            cartItem.DinnerPrice = updateCartItemDTO.DinnerPrice;
            cartItem.BikeRentPrice = updateCartItemDTO.BikeRentPrice;
            cartItem.UpdatedAt = DateTime.UtcNow;

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return await GetCartItemsByCartId(cartItem.CartItemId);
        }


        public async Task DeleteCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

    }
}
