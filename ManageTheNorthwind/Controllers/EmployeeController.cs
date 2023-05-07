using ManageTheNorthwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageTheNorthwind.Controllers
{
    public class EmployeeController : Controller
    {
        NorthwindContext _context = new();

        public IActionResult Index()
        {
            return View(_context.Employees.ToList());
        }
        public IActionResult Profile(int id)
        {
           Employee employee= _context.Employees.Where(e=>e.EmployeeId== id).FirstOrDefault();
            return View(employee);
        }
        public async Task<IActionResult> ListSales(int id)
        {
            var orders =  _context.Orders.Where(e => e.EmployeeId == id).Include(x => x.Customer).ToList().GroupBy(x => x.Customer).Select(u => new { MusteriId = u.Key.CustomerId, MusteriName = u.Key.CompanyName, MusteriCountry = u.Key.Country, ToplamSipariş = u.Count() }); 

            ViewBag.toplamsiparis= await _context.Orders.Where(e => e.EmployeeId == id).CountAsync();
            return View(orders);
        }
        public IActionResult Create()
        {
            return View();
        } 
        public IActionResult Edit(int id)
        {
            var emp = _context.Employees.Find(id);
            return View(emp);
        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
            _context.Employees.Add(emp);
            _context.SaveChanges();
                return RedirectToAction("Index");  
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            _context.Employees.Remove(_context.Employees.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
