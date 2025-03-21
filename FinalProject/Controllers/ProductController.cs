using FinalProject.Data;
using FinalProject.Dtos.Product;
using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly IIdentityService _identityService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IIdentityService identityService, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _productService = productService;
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
        public IActionResult ProductDetailPage(Guid id)
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
        public async Task<IActionResult> AddProductAsync(CreateProductDto createProductDto)
        {
            var applicationUser = await _userManager.GetUserAsync(User);

            //var category = _applicationDbContext.Categories.FirstOrDefault(x => x.Id == Guid.Parse(createProductDto.CategoryId));

            ////category ??= new Category { Name = createProductDto.CategoryId, Id = Guid.NewGuid() };

            ////if (category == null)
            ////{
            ////    category = new Category { Name = createProductDto.CategoryName, Id = Guid.NewGuid() };

            ////    //_applicationDbContext.Add(_category);
            ////    //_applicationDbContext.SaveChanges();
            ////}

            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                ImageUrl = createProductDto.ImageUrl,
                CreatedByUserId = Guid.Parse(applicationUser.Id),
                IdentityUser = applicationUser,
                IdentityUserId = Guid.Parse(applicationUser.Id),
                CreatedOn = DateTimeOffset.UtcNow,
            };

            //category.ProductCategories.Add(new ProductCategory 
            //{ 
            //    Product = product, 
            //    CategoryId = category.Id,
            //    ProductId = product.Id, 
            //    Category = category 
            //});

            try
            {
                _applicationDbContext.Add(product);
                _applicationDbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }

            //var fileId = await _blobService.UploadAsync(file.OpenReadStream(), "documents", file.ContentType, cancellationToken);



        }
    }
    }
