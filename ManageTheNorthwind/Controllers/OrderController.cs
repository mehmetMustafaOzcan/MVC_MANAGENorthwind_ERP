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
            List<SelectListItem> Shipper =_context.Shippers.Select(x=>new SelectListItem { Value = x.ShipperId.ToString(),Text=x.CompanyName}).ToList();
            ViewBag.EmployeeId = Employee;
            ViewBag.CustomerId = Customer;
            ViewBag.ShipVia = Shipper;
            

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();

            return RedirectToAction("CreateOD", order);
        }
        public async Task<IActionResult> CreateOD(Order order)
        {
            List<SelectListItem> Products = _context.Products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.ProductName, }).ToList();
            ViewBag.ProductId = Products;
            ViewBag.order = order;
            ViewBag.Product = _context.Products.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOD(OrderDetail orderdetail)
        {
            await _context.OrderDetails.AddAsync(orderdetail);
            Order order=await _context.Orders.Where(x => x.OrderId == orderdetail.OrderId).FirstAsync();
            _context.SaveChanges();
            return RedirectToAction("CreateOD", order);
        }
        public async Task<IActionResult> CreateOD3()
        {

            //Order order = await _context.Orders.Where(x => x.OrderId == orderdetail.OrderId).FirstAsync();
            //  _context.SaveChanges();
            ViewBag.Product = _context.Products.ToList();
            return View();
        }
        [HttpPost]
        public async void CreateOD3(OrderDetail orderdetails)
        {
           
          
        }
       
    }
}
