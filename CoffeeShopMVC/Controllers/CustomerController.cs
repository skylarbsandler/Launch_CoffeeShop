using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;

namespace CoffeeShopMVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public CustomersController(CoffeeShopMVCContext Context)
        {
            _context = Context;
        }

        public IActionResult Index()
        {
            var customers = _context.Customers;

            return View(customers);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("/Customers")]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}