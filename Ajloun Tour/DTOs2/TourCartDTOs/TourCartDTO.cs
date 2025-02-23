using Ajloun_Tour.DTOs.UsersDTOs;

namespace Ajloun_Tour.DTOs2.TourCartDTOs
{
    public class TourCartDTO
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
        public UsersDTO User { get; set; }  
    }
}
