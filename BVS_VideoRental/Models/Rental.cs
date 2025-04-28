using System;

namespace BVS_VideoRental.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public int VideoId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable because not returned yet sometimes
    }
}
