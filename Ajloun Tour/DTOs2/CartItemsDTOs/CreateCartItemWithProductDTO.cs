using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.CartItemsDTOs
{
    public class CreateCartItemWithProductDTO
    {
        [Required]
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime SelectedDate { get; set; }
     

        public int? ProductId { get; set; }
    }
}
