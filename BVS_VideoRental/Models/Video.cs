namespace BVS_VideoRental.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; } // Only "VCD" or "DVD"
        public int Quantity { get; set; }
        public int MaxRentalDays { get; set; }
    }
}
