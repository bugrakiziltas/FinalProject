using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IIdentityService _identityService;
        private readonly IOrderRepository _orderRepository;
        public OrderController(IShoppingCartRepository shoppingCartRepository, IOrderRepository orderRepository, IIdentityService ıdentityService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _identityService = ıdentityService;
            _orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("/purchases")]
        public async Task<IActionResult> Purchases()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = await _orderRepository.GetOrdersByUserId(Guid.Parse(userId));

            return View(orders);
        }

    }
}
