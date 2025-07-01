using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IIdentityService _identityService;
        private readonly IOrderRepository _orderRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IOrderRepository orderRepository,IIdentityService ıdentityService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _identityService = ıdentityService;
            _orderRepository = orderRepository;
        }
        [HttpGet]
        [Route("/cart")]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var user = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var userId = user.Id.ToString();
            var items =await  _shoppingCartRepository.GetShoppingCartItems(Guid.Parse(userId));
            ViewBag.Total=items.Select(x=>x.Product.Price).Sum();
            return View(items);
        }
        [HttpGet]
        [Route("/cart/{id:guid}")]
        public async Task<IActionResult> RemoveFromShoppingCart([FromRoute] Guid id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await _shoppingCartRepository.RemoveFromShoppingCart(id, Guid.Parse(userId)) == true)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Something went wrong");
        }
        [HttpPost]
        [Route("/order")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var user = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var userId = user.Id.ToString();
            var cartItems = await _shoppingCartRepository.GetShoppingCartItems(Guid.Parse(userId));

            var domain = $"{Request.Scheme}://{Request.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.Product.Price * 100), 
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = 1
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{domain}/order/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/cart"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }
        [HttpGet]
        [Route("/order/success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var user = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
                var userId = user.Id.ToString();
                var cartItems = await _shoppingCartRepository.GetShoppingCartItems(Guid.Parse(userId));

                var order = new Order
                {
                    IdentityUserId = Guid.Parse(userId),
                    CreatedOn = DateTime.Now,
                    Total = cartItems.Sum(x => x.Product.Price),
                    BuyedProducts = cartItems.Select(x => x.Product).ToList(),
                };

                if(await _orderRepository.AddOrder(order) == true)
                {
                    if(await _shoppingCartRepository.ClearCart(Guid.Parse(userId)) == true)
                    {
                        return RedirectToAction("Index", "ShoppingCart");
                    }
                    return BadRequest("Something went wrong");
                }

                return BadRequest("Something went wrong");
            }

            return BadRequest("Payment not confirmed.");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartItemCount()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (!claimsIdentity.IsAuthenticated)
            {
                var count1 = 0;
                return Json(new { count1 });
            }
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var count = await _shoppingCartRepository.CountAsync(Guid.Parse(userId));
            return Json(new { count });
        }
    }
}
