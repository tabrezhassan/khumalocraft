using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using khumalocraft.Data;
using khumalocraft.Models;

namespace khumalocraft.Controllers
{
    [Authorize]
    public class MyWorkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyWorkController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var craftworks = await _context.Products.ToListAsync();
            return View(craftworks);
        }

        [HttpGet]
        public IActionResult BuyNow()
        {
            // Render the BuyNow view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(int productId, string name, string email, string address, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Availability >= quantity && quantity > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user?.Id; // Ensure userId is a string

                // Create a new order
                var order = new Order
                {
                    UserId = userId, // Set the logged-in user id
                    OrderDate = DateTime.Now,
                    TotalAmount = (int)(product.Price * quantity),
                    OrderDetails = new List<OrderDetails>
                    {
                        new OrderDetails
                        {
                            ProductId = productId,
                            Quantity = quantity,
                            Price = (int)product.Price
                        }
                    }
                };

                // Add the order to the database
                _context.Orders.Add(order);

                // Decrement the availability count
                product.Availability -= quantity;

                // Save changes to the database
                await _context.SaveChangesAsync();

                ViewBag.Message = "Order submitted successfully!";
            }
            else
            {
                ViewBag.Message = "Product not available in the requested quantity.";
            }

            return View();
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id; // Ensure userId is a string

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return View(orders);
        }
    }
}
