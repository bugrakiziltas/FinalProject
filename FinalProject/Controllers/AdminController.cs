using FinalProject.Dtos.Product;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminController : Controller
    {
        private readonly IProductService productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            this.productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllProductsAsync();
            return View(products);
        }
        [HttpGet]
        [Route("/create")]
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }
        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto requestModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (requestModel.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                Directory.CreateDirectory(uploadsFolder);
                var imageUrl = Guid.NewGuid().ToString() + requestModel.Image.FileName;
                string serverFolder = Path.Combine(uploadsFolder, imageUrl);
                using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                {
                    requestModel.Image.CopyTo(fileStream);
                }
                if (await productService.CreateProductAsync(requestModel, userId, imageUrl) == true) return RedirectToAction("Index");
                return BadRequest("Something went wrong");
            }
            return BadRequest("Something went wrong");
        }
        [HttpGet]
        [Route("/Update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null) return BadRequest("The product does not exists");
            var productModel = new UpdateProductDto
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
            };
            ViewBag.ImageUrl = product.ImageUrl;
            return View(productModel);
        }
        [HttpPost]
        [Route("/Update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto requestModel)
        {
            var imageUrl = new string(String.Empty);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (requestModel.Image != null)
            {
                imageUrl = Guid.NewGuid().ToString() + requestModel.Image.FileName;
                if (await productService.UpdateProductAsync(requestModel, imageUrl, userId, CancellationToken.None) == true) return RedirectToAction("Index");
                return BadRequest("Something went wrong");
            }
            if(await productService.UpdateProductAsync(requestModel, imageUrl, userId, CancellationToken.None) ==true) return RedirectToAction("Index");
            return BadRequest("Something went wrong");
        }
        [HttpPost]
        [Route("/Delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return BadRequest("The product does not exists");
            }
            if(await productService.DeleteProductAsync(id, CancellationToken.None)==true) return RedirectToAction("Index");
            return BadRequest("Something went wrong");
        }

    }
}
