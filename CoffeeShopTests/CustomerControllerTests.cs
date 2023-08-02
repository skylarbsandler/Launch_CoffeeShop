using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;
using System.Net;

namespace CoffeeShopTests.FeatureTests
{
    [Collection("Controller Tests")]
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private CoffeeShopMVCContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoffeeShopMVCContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new CoffeeShopMVCContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Test_Index_ReturnsViewWithCustomers()
        {
            var context = GetDbContext();
            context.Customers.Add(new Customer { Name = "Jeff", EmailAddress = "jeff@mail.com" });
            context.Customers.Add(new Customer { Name = "Yeff", EmailAddress = "yeff@mail.com" });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Customers");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("Jeff", html);
            Assert.Contains("Yeff", html);
            Assert.Contains("yeff@mail.com", html);
            Assert.Contains("jeff@mail.com", html);
        }
        
        [Fact]
        public async Task _Test_New_ReturnsViewWithForm()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/customers/new");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"<form method=\"post\" action=\"/customers\">", html);
        }

        [Fact]
        public async void Test_Create_AddsItemToDatabase()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            context.Customers.Add(new Customer { Name = "Jeff", EmailAddress = "jeff@mail.com" });
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                { "Name", "Jeff" },
                { "EmailAddress", "jeff@mail.com" }
            };

            var response = await client.PostAsync("/customers", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Jeff", html);

            Assert.Equal(2, context.Customers.Count());
            Assert.Equal("Jeff", context.Customers.First().Name);
        }
    }
}
