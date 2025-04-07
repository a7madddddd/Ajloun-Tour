using Ajloun_Tour.DTOs.BookingsDTOs;
using Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs;
using Ajloun_Tour.DTOs2.CartItemsDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.CodeAnalysis;
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
                    BookingId = ci.BookingId,
                    ProductId = ci.ProductId
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
                ProductId = cartItem.ProductId,
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
            // Get the user ID associated with this cart
            var userId = await _context.Carts
                .Where(tc => tc.CartId == cartId)
                .Select(tc => tc.UserId)
                .FirstOrDefaultAsync();

            if (userId == null)
                return new List<CartItemDTO>();

            // Get existing cart items
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Booking)
                .Select(ci => new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    CartId = ci.CartId,
                    TourId = ci.TourId,
                    PackageId = ci.PackageId,
                    OfferId = ci.OfferId,
                    ProductId = ci.ProductId,
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
                    BookingId = ci.BookingId,

                    BookingDetails = ci.Booking != null ? new BookingDTO
                    {
                        BookingId = ci.Booking.BookingId,
                        TourId = ci.Booking.TourId,
                        PackageId = ci.Booking.PackageId,
                        OfferId = ci.Booking.OfferId,
                        UserId = ci.Booking.UserId,
                        BookingDate = ci.Booking.BookingDate,
                        NumberOfPeople = ci.Booking.NumberOfPeople,
                        TotalPrice = ci.Booking.TotalPrice,
                        Status = ci.Booking.Status,
                        CreatedAt = ci.Booking.CreatedAt,
                        SelectedOptions = _context.BookingOptionSelections
                            .Where(bos => bos.BookingId == ci.Booking.BookingId)
                            .Include(bos => bos.Option)
                            .Select(bos => new BookingOptionSelectionDTO
                            {
                                BookingId = bos.BookingId,
                                OptionId = bos.OptionId,
                                SelectionId = bos.SelectionId,
                                OptionName = bos.Option.OptionName,
                                OptionPrice = bos.Option.OptionPrice
                            })
                            .ToList()
                    } : null
                })
                .ToListAsync();

            // Get bookings that aren't associated with cart items
            var bookingsNotInCart = await _context.Bookings
                .Where(b => b.UserId == userId && !_context.CartItems.Any(ci => ci.BookingId == b.BookingId))
                .Select(b => new CartItemDTO
                {
                    CartItemId = 0,
                    CartId = cartId,
                    TourId = b.TourId,
                    PackageId = b.PackageId,
                    OfferId = b.OfferId,
                    Quantity = 1,
                    Price = b.TotalPrice,
                    SelectedDate = b.BookingDate,
                    NumberOfPeople = b.NumberOfPeople,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.CreatedAt,
                    BookingId = b.BookingId,
                    IsFromBooking = true,
                    BookingDetails = new BookingDTO
                    {
                        BookingId = b.BookingId,
                        TourId = b.TourId,
                        PackageId = b.PackageId,
                        OfferId = b.OfferId,
                        UserId = b.UserId,
                        BookingDate = b.BookingDate,
                        NumberOfPeople = b.NumberOfPeople,
                        TotalPrice = b.TotalPrice,
                        Status = b.Status,
                        CreatedAt = b.CreatedAt,
                        SelectedOptions = _context.BookingOptionSelections
                            .Where(bos => bos.BookingId == b.BookingId)
                            .Include(bos => bos.Option)
                            .Select(bos => new BookingOptionSelectionDTO
                            {
                                BookingId = bos.BookingId,
                                OptionId = bos.OptionId,
                                SelectionId = bos.SelectionId,
                                OptionName = bos.Option.OptionName,
                                OptionPrice = bos.Option.OptionPrice
                            })
                            .ToList()
                    }
                })
                .ToListAsync();

            return cartItems.Concat(bookingsNotInCart);
        }
        public async Task<CartItemDTO> AddCartItemAsync(CreateCartItemDTO createCartItemDTO)
        {
            Booking booking = null;

            if (createCartItemDTO.BookingId != null && createCartItemDTO.BookingId != 0)
            {
                booking = await _context.Bookings
                    .Where(b => b.BookingId == createCartItemDTO.BookingId)
                    .FirstOrDefaultAsync();

                if (booking == null)
                {
                    throw new Exception($"لم يتم العثور على الحجز برقم BookingId = {createCartItemDTO.BookingId}");
                }
            }

            var cartItem = new CartItem
            {
                CartId = createCartItemDTO.CartId,
                TourId = (booking?.TourId != null && booking.TourId != 0) ? booking.TourId : null,
                PackageId = (booking?.PackageId != null && booking.PackageId != 0) ? booking.PackageId : null,
                OfferId = (booking?.OfferId != null && booking.OfferId != 0) ? booking.OfferId : null,
                ProductId = createCartItemDTO.ProductId ?? 0,
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
                BookingId = (createCartItemDTO.BookingId != 0) ? createCartItemDTO.BookingId : null
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

        public async Task<CartItemDTO> AddCartItemByProductAsync(CreateCartItemWithProductDTO dto)
        {
            var cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = dto.Price,
                SelectedDate = dto.SelectedDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return new CartItemDTO
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId ,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                SelectedDate = cartItem.SelectedDate,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt
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
            cartItem.ProductId  = updateCartItemDTO.ProductId;

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
