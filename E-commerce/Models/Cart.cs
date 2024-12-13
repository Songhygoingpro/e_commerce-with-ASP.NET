namespace E_commerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }      // Primary Key
        public int UserId { get; set; }     // Foreign Key
        public int ProductId { get; set; }  // Foreign Key
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
