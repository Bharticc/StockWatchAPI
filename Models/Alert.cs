namespace StockWatchAPI.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StockId { get; set; }
        public decimal AlertPrice { get; set; }
        public bool IsTriggered { get; set; }

        // Marking these properties as nullable:
        public User? User { get; set; }
        public Stock? Stock { get; set; }
    }
}
