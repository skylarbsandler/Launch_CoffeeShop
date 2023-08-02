using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        [Route("/Customers/Details/{Id:int}")]
        public IActionResult Show(int Id)
        {
            var customer = _context.Customers.Find(Id);

            //customer = customer.Where(c => c.Id == Id);

            //ViewData["CustomerData"] = customer.Select(c => c.Orders.Select(o => o.Items)).ToList();

            return View(customer);
        }
    }
}