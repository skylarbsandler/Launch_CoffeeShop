﻿namespace CoffeeShopMVC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
