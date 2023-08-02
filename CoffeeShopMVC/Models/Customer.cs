namespace CoffeeShopMVC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public int TotalDollarsSpent()
        {
            var priceTotal = 0;
            this.Orders.ForEach(order => order.Items.ForEach(item => priceTotal += item.PriceInCents));
            return priceTotal;
        }
    }
}
