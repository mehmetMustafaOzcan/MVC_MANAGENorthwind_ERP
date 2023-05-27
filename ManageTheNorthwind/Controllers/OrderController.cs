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
        public async Task<IActionResult> Delete(int id)
        {
            Order order=await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateOD", order);
        }
        public async Task<IActionResult> CreateOD(int id, Order order)
       {
            List<SelectListItem> Products = await _context.Products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.ProductName, }).ToListAsync();
            ViewBag.ProductId = Products;
            ViewBag.Product =await _context.Products.ToListAsync();
           var ord= id>0? await _context.Orders.FindAsync(id):order;
              ViewBag.order = ord;
            
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOD(OrderDetail orderdetail)
        {
            Product product =await _context.Products.FindAsync(orderdetail.ProductId);
            var ord = await _context.OrderDetails.Where(x =>x.OrderId == orderdetail.OrderId).Select(p=>p.ProductId).ToListAsync();
            if (!ord.Contains(orderdetail.ProductId))
            {
                orderdetail.UnitPrice =(decimal)product.UnitPrice;
                await _context.OrderDetails.AddAsync(orderdetail);
              
                _context.SaveChanges();
                return RedirectToAction("CreateOD", orderdetail.OrderId);
            }
            else
            {
                OrderDetail od =await _context.OrderDetails.FirstOrDefaultAsync(x => x.OrderId == orderdetail.OrderId && x.ProductId == orderdetail.ProductId);

                od.UnitPrice = (od.UnitPrice / od.Quantity) * (od.Quantity + orderdetail.Quantity);
               od.Quantity+=orderdetail.Quantity;
                 _context.OrderDetails.Update(od);
                _context.SaveChanges();
                return RedirectToAction("CreateOD", orderdetail.OrderId);
            }
           

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
