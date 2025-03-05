using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PizzaStar.Data;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Pages;
using PizzaStar.ViewModels;
using PizzaStar.Data.Helpers;

namespace PizzaStar.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;
        private readonly EmailSender _emailSender;
        private readonly ApplicationContext _context;

        public HomeController(IProduct products, ICategory categories, EmailSender emailSender, ApplicationContext context)
        {
            _products = products;
            _categories = categories;
            _emailSender = emailSender;
            _context = context;
        }

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index(QueryOptions options, int categoryId)
        {
            if (categoryId != 0)
            {
                ViewBag.CategoryId = categoryId;
                
                var currentCategory = await _categories.GetCategoryAsync(categoryId);
                if (currentCategory != null)
                {
                    ViewData["Title"] = currentCategory.Name;
                }
                return View(_products.GetAllProductsByCategory(options, categoryId));
            }
            else
            {
                ViewData["Title"] = "Главная";
                return View(_products.GetAllProducts(options));
            }
        }

        [Route("/product")]
        [HttpGet]
        public async Task<IActionResult> GetProduct(int productId, string? ReturnUrl)
        {
            if (productId > 0)
            {
                var currentProduct = await _products.GetProductWithCategoryAsync(productId);
                if (currentProduct != null)
                {
                    ViewBag.CategoryId = double.NaN;
                    return View(new CurrentProductViewModel
                    {
                        Product = currentProduct,
                        ReturnUrl = ReturnUrl
                    });
                }
            }

            return NotFound();
        }

        [Route("/contact")]
        [HttpGet]
        public IActionResult Contact(string? ReturnUrl)
        {
            return View(new ContactFormViewModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Contact(ContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<string> adminEmails = _context.Contacts.Select(c => c.Email).ToList();
                _emailSender.SendClientMessage(model.Email, model.Name, model.Enquiry, adminEmails);
            }
            return View("Index");
        }
    }
}
