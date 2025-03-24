﻿using FinalProject.Data;
using FinalProject.Dtos.Product;
using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddProduct()
        {
            var createProductDto = new CreateProductDto();
            return View(createProductDto);

        }

        //[HttpGet]
        //public async Task<IActionResult> CategoryIndexAsync()
        //{
        //    var cat = await _categoryService.GetAllCategoriesAsync();
        //    return View(cat);

        //}

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> ProductDetailPage(Guid id)
        {
            var product = _applicationDbContext.Products.FirstOrDefault(x => x.Id == id);

            return View(product);

        }

        //[HttpGet]
        //[Route("categories/{categoryName}")]
        //public IActionResult ProductCategoryPage(string categoryName)
        //{
        //    var products = _applicationDbContext.Products.Where(x => x.Category.Name == categoryName).ToList();
        //    return View(products);

        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(Product product)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var identityUser = await _identityService.GetUserByIdAsync(userId);
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
    }
    }
