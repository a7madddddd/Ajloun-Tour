using Ajloun_Tour.DTOs2.CartItemsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Repositories.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly MyDbContext _context;

        public CartItemRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItemDTO>> GetAllCartItemsAsync()
        {
            return await _context.CartItems
                .Select(ci => new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    CartId = ci.CartId,
                    TourId = ci.TourId,
                    PackageId = ci.PackageId,
                    OfferId = ci.OfferId,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    SelectedDate = ci.SelectedDate,
                    NumberOfPeople = ci.NumberOfPeople,
                    Option1 = ci.Option1,
                    Option2 = ci.Option2,
                    Option3 = ci.Option3,
                    Option4 = ci.Option4,
                    Option1Price = ci.Option1Price,
                    Option2Price = ci.Option2Price,
                    Option3Price = ci.Option3Price,
                    Option4Price = ci.Option4Price,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt,
                    BookingId = ci.BookingId
                })
                .ToListAsync();
        }

        public async Task<CartItemDTO> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return null;

            return new CartItemDTO
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                TourId = cartItem.TourId,
                PackageId = cartItem.PackageId,
                OfferId = cartItem.OfferId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                SelectedDate = cartItem.SelectedDate,
                NumberOfPeople = cartItem.NumberOfPeople,
                Option1 = cartItem.Option1,
                Option2 = cartItem.Option2,
                Option3 = cartItem.Option3,
                Option4 = cartItem.Option4,
                Option1Price = cartItem.Option1Price,
                Option2Price = cartItem.Option2Price,
                Option3Price = cartItem.Option3Price,
                Option4Price = cartItem.Option4Price,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt,
                BookingId = cartItem.BookingId
            };
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Select(ci => new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    CartId = ci.CartId,
                    TourId = ci.TourId,
                    PackageId = ci.PackageId,
                    OfferId = ci.OfferId,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    SelectedDate = ci.SelectedDate,
                    NumberOfPeople = ci.NumberOfPeople,
                    Option1 = ci.Option1,
                    Option2 = ci.Option2,
                    Option3 = ci.Option3,
                    Option4 = ci.Option4,
                    Option1Price = ci.Option1Price,
                    Option2Price = ci.Option2Price,
                    Option3Price = ci.Option3Price,
                    Option4Price = ci.Option4Price,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt,
                    BookingId = ci.BookingId
                })
                .ToListAsync();
        }

        public async Task<CartItemDTO> AddCartItemAsync(CreateCartItemDTO createCartItemDTO)
        {
            // Retrieve the booking using the provided BookingId
            var booking = await _context.Bookings
                .Where(b => b.BookingId == createCartItemDTO.BookingId)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            // Create a new CartItem based on the Booking information
            var cartItem = new CartItem
            {
                CartId = createCartItemDTO.CartId,
                TourId = booking.TourId ?? 0,  
                PackageId = booking.PackageId ?? 0,  
                OfferId = booking.OfferId ?? 0, 
                Quantity = createCartItemDTO.Quantity,
                Price = createCartItemDTO.Price,
                SelectedDate = createCartItemDTO.SelectedDate,
                NumberOfPeople = createCartItemDTO.NumberOfPeople,
                Option1 = createCartItemDTO.Option1,
                Option2 = createCartItemDTO.Option2,
                Option3 = createCartItemDTO.Option3,
                Option4 = createCartItemDTO.Option4,
                Option1Price = createCartItemDTO.Option1Price,
                Option2Price = createCartItemDTO.Option2Price,
                Option3Price = createCartItemDTO.Option3Price,
                Option4Price = createCartItemDTO.Option4Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BookingId = createCartItemDTO.BookingId 
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return new CartItemDTO
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                TourId = cartItem.TourId,
                PackageId = cartItem.PackageId,
                OfferId = cartItem.OfferId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                SelectedDate = cartItem.SelectedDate,
                NumberOfPeople = cartItem.NumberOfPeople,
                Option1 = cartItem.Option1,
                Option2 = cartItem.Option2,
                Option3 = cartItem.Option3,
                Option4 = cartItem.Option4,
                Option1Price = cartItem.Option1Price,
                Option2Price = cartItem.Option2Price,
                Option3Price = cartItem.Option3Price,
                Option4Price = cartItem.Option4Price,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt,
                BookingId = cartItem.BookingId
            };
        }

        public async Task<CartItemDTO> UpdateCartItemAsync(int id, UpdateCartItemDTO updateCartItemDTO)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return null;

            cartItem.TourId = updateCartItemDTO.TourId ?? cartItem.TourId;
            cartItem.PackageId = updateCartItemDTO.PackageId ?? cartItem.PackageId;
            cartItem.OfferId = updateCartItemDTO.OfferId ?? cartItem.OfferId;
            cartItem.Quantity = updateCartItemDTO.Quantity;
            cartItem.Price = updateCartItemDTO.Price;
            cartItem.SelectedDate = updateCartItemDTO.SelectedDate;
            cartItem.NumberOfPeople = updateCartItemDTO.NumberOfPeople;
            cartItem.Option1 = updateCartItemDTO.Option1;
            cartItem.Option2 = updateCartItemDTO.Option2;
            cartItem.Option3 = updateCartItemDTO.Option3;
            cartItem.Option4 = updateCartItemDTO.Option4;
            cartItem.Option1Price = updateCartItemDTO.Option1Price;
            cartItem.Option2Price = updateCartItemDTO.Option2Price;
            cartItem.Option3Price = updateCartItemDTO.Option3Price;
            cartItem.Option4Price = updateCartItemDTO.Option4Price;
            cartItem.UpdatedAt = DateTime.UtcNow;
            cartItem.BookingId = updateCartItemDTO.BookingId;

            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new CartItemDTO
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                TourId = cartItem.TourId,
                PackageId = cartItem.PackageId,
                OfferId = cartItem.OfferId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                SelectedDate = cartItem.SelectedDate,
                NumberOfPeople = cartItem.NumberOfPeople,
                Option1 = cartItem.Option1,
                Option2 = cartItem.Option2,
                Option3 = cartItem.Option3,
                Option4 = cartItem.Option4,
                Option1Price = cartItem.Option1Price,
                Option2Price = cartItem.Option2Price,
                Option3Price = cartItem.Option3Price,
                Option4Price = cartItem.Option4Price,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt,
                BookingId = cartItem.BookingId
            };
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItemDTO>> UpdateCartItemsBookingIdAsync(int[] cartItemIds, int bookingId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => cartItemIds.Contains(ci.CartItemId))
                .ToListAsync();

            foreach (var cartItem in cartItems)
            {
                cartItem.BookingId = bookingId;
                cartItem.UpdatedAt = DateTime.UtcNow;
                _context.Entry(cartItem).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return cartItems.Select(ci => new CartItemDTO
            {
                CartItemId = ci.CartItemId,
                CartId = ci.CartId,
                TourId = ci.TourId,
                PackageId = ci.PackageId,
                OfferId = ci.OfferId,
                Quantity = ci.Quantity,
                Price = ci.Price,
                SelectedDate = ci.SelectedDate,
                NumberOfPeople = ci.NumberOfPeople,
                Option1 = ci.Option1,
                Option2 = ci.Option2,
                Option3 = ci.Option3,
                Option4 = ci.Option4,
                Option1Price = ci.Option1Price,
                Option2Price = ci.Option2Price,
                Option3Price = ci.Option3Price,
                Option4Price = ci.Option4Price,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt,
                BookingId = ci.BookingId
            });
        }
    }
}
