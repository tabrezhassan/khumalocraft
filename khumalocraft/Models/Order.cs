using khumalocraft.Models;

namespace khumalocraft.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
