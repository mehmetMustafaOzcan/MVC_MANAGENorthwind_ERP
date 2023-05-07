using ManageTheNorthwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManageTheNorthwind.Controllers
{
    public class OrderController : Controller
    {
        NorthwindContext _context = new();
        public IActionResult Index()
        {
            //ViewBag.prices = _context.OrderDetails.GroupBy(x => x.OrderId).Select(x => x.Sum(x => (x.Quantity * x.UnitPrice)) ).ToList();
            return View(_context.Orders.Include(x => (x.Customer)).Include(x => x.OrderDetails).OrderByDescending(x => x.OrderDate));
        }
        public IActionResult Price()
        {
            var prices = _context.OrderDetails.GroupBy(x => x.OrderId).Select(x => new { price = x.Sum(x => (x.Quantity * x.UnitPrice)) });
            return View(prices);
        }
        public IActionResult Details(int id)
        {
            var order = _context.Orders.Where(x => x.OrderId == id).Include(x => x.OrderDetails).ThenInclude(x => x.Product).FirstOrDefault();
            //  ViewBag.Orderdetails=order.OrderDetails;
            return View(order);
        }
        public IActionResult Delete(int id)
        {
            return View();
        } 
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> Employee =_context.Employees.Select(x=>new SelectListItem { Value = x.EmployeeId.ToString(),Text=x.FirstName}).ToList();
            List<SelectListItem> Customer =_context.Customers.Select(x=>new SelectListItem { Value = x.CustomerId.ToString(),Text=x.CompanyName}).ToList();
            
            ViewBag.EmployeeId = Employee;
            ViewBag.CustomerId = Customer;
            return View();
        }
    }
}
