using ManageTheNorthwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;

namespace ManageTheNorthwind.Controllers
{
    public class ReportsController : Controller
    {
        NorthwindContext _context = new();
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SalesBy(string filter)
        {
            //  var orders = _context.Orders.Include(x => x.OrderDetails).GroupBy(o => new { ((DateTime)o.OrderDate).Year, ((DateTime)o.OrderDate).Month });
            // var Orders2 = orders.Select(aOrderlist => new { aOrderlist.Key.Year, aOrderlist.Key.Month, a= aOrderlist.Select(order => new { order.OrderDate, order.Customer, order.ShipRegion, price = order.OrderDetails.GroupBy(x => x.OrderId).Select(x => new { total = x.Sum(x => (x.UnitPrice * x.Quantity)) }) }) }); 
            if (filter == "Month")
            {

                var result = (from o in _context.Orders
                              join od in _context.OrderDetails on o.OrderId equals od.OrderId
                              group od by new { ((DateTime)o.OrderDate).Year, ((DateTime)o.OrderDate).Month } into g
                              select new { Year = g.Key.Year, Month = g.Key.Month, TotalSales = g.Sum(od => od.UnitPrice * od.Quantity) }).OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
                return View(Json(result));
            }
            else if (filter == "Year")
            {
                var result = (from o in _context.Orders
                              join od in _context.OrderDetails on o.OrderId equals od.OrderId
                              group od by new { ((DateTime)o.OrderDate).Year } into g
                              select new { Year = g.Key.Year, TotalSales = g.Sum(od => od.UnitPrice * od.Quantity) }).OrderBy(x => x.Year).ToList();
                return View(Json(result));
            }
            else if (filter == "Employee")
            {

                var r2 = await (from o in _context.Orders
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                join emp in _context.Employees on o.EmployeeId equals emp.EmployeeId
                                group od by new { o.EmployeeId, o.Employee.FirstName } into e
                                select new { e.Key.EmployeeId, e.Key.FirstName, Total = e.Sum(x => x.UnitPrice * x.Quantity), }).ToListAsync();
                return View(Json(r2));
            }
            else if (filter == "Product")
            {

                var r2 = await (from o in _context.Orders
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                join x in _context.Products on od.ProductId equals x.ProductId
                                join c in _context.Categories on x.CategoryId equals c.CategoryId
                                group od by new { ıd = od.ProductId, name = x.ProductName, catname = c.CategoryName } into p
                                select new { Product_Name=p.Key.name, Category_Name= p.Key.catname, TotalQuantity = p.Sum(x => x.Quantity) }).OrderBy(x=>x.TotalQuantity).ToListAsync();

                
                return View(Json(r2));


            }


            return View();
        }
    }
}
