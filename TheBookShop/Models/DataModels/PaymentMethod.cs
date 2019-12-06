namespace TheBookShop.Models.DataModels
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}