using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDataAccess _dataAccess;

        public HomeController(ProductDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            // Fetch all products
            List<Product> products = _dataAccess.GetAllProducts();
            return View(products); // Pass products to the view
        }
    }
}
