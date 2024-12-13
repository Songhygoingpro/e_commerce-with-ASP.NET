using E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDataAccess _dataAccess;
        private readonly string _imagePath;

        public ProductController(ProductDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }

        // Displays all products.
        public IActionResult Index()
        {
            var products = _dataAccess.GetAllProducts();
            return View(products);
        }

        // Displays the form for creating a new product.
        public IActionResult Create()
        {
            return View();
        }

        // Handles the submission of the create product form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Ensure the "images" folder exists
                    if (!Directory.Exists(_imagePath))
                    {
                        Directory.CreateDirectory(_imagePath);
                    }

                    // Save the image file
                    var filePath = Path.Combine(_imagePath, image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    // Set image path in the product
                    product.Image = "/images/" + image.FileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "The Image field is required.");
                    return View(product);
                }

                _dataAccess.AddProduct(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // Displays the form to edit an existing product.
        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _dataAccess.GetAllProducts().FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Pass the product to the view
        }


        // Handles the submission of the edit product form.
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,Title,Quantity,Price,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataAccess.UpdateProduct(product); // Assume this method updates the product in the database
                    return RedirectToAction(nameof(Index)); // Redirect to the product list after successful update
                }
                catch (Exception)
                {
                    // Handle the error or log it, e.g.:
                    ModelState.AddModelError("", "Unable to update the product. Please try again.");
                }
            }
            return View(product); // If validation fails, show the form again with validation messages
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _dataAccess.GetAllProducts().FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Title = product.Title;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Price = product.Price;
                existingProduct.Image = product.Image;
                // Save changes (for example, if you're using a database, call a database save method)
            }
        }



        // Displays the confirmation page for deleting a product.
        public IActionResult Delete(int id)
        {
            var product = _dataAccess.GetAllProducts().FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Handles the deletion of a product.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, bool confirm)
        {
            if (!confirm)
            {
                // If the confirmation is not true, just redirect to the product list
                return RedirectToAction("Index");
            }

            var product = _dataAccess.GetAllProducts().FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Delete the associated image file if it exists
            if (!string.IsNullOrEmpty(product.Image))
            {
                var filePath = Path.Combine(_imagePath, Path.GetFileName(product.Image));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Delete the product from the data source
            _dataAccess.DeleteProduct(id);

            return RedirectToAction("Index");
        }
    }
}
