using FinalProject.Data;
using FinalProject.Dtos.Product;
using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IIdentityService _identityService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IShoppingCartRepository shoppingCartRepository ,IIdentityService identityService, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _productService = productService;
            _shoppingCartRepository = shoppingCartRepository;
            _identityService = identityService;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;

        }


        // GET: ProductController
        public async Task<ActionResult> IndexAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            return View(products);
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> ProductDetailPage(Guid id)
        {
            // TODO : Make this async by adding await
            var product = _applicationDbContext.Products.FirstOrDefault(x => x.Id == id);

            return View(product);

        }

        [HttpGet]
        [Route("category/{categoryName}")]
        public IActionResult CategoryIndex(string categoryName)
        {
            // TODO : Check categoryName is valid
            var products = _applicationDbContext.Products.Include(x=> x.Category).Where(x => x.Category.CategoryName == categoryName).ToList();
            return View(products);

        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var results = _applicationDbContext.Products.Include(x => x.Category)
                .Where(p => p.Name.Contains(query) || p.Category.CategoryName.Contains(query))
                .Take(6)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    category = new
                    {
                        categoryName = p.Category.CategoryName
                    },
                    url = Url.Action("ProductDetailPage", "Product", new { id = p.Id })
                })
                .ToList();

            return Json(results);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddProduct()
        {
            var createProductDto = new CreateProductDto();
            return View(createProductDto);

        }

        [HttpGet]
        [Authorize]
        public IActionResult AddToCart()
        {
            // Redirect to product list or show an error message
            TempData["ErrorMessage"] = "Please select a product to add to cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(Product product)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var identityUser = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var userId = identityUser.Id.ToString();
            var shoppingCartItem = new ShoppingCart
            {
                ProductId = product.Id,
                IdentityUserId = Guid.Parse(userId),
            };
            if (await _shoppingCartRepository.AddToCart(shoppingCartItem) == true)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Something went wrong");
        }



        //[HttpGet]
        //public async Task<IActionResult> CategoryIndexAsync()
        //{
        //    var cat = await _categoryService.GetAllCategoriesAsync();
        //    return View(cat);

        //}

    }
}
