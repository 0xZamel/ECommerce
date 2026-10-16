    using ECommerce.DataAccess.Repository.IRepository;
    using ECommerce.Models;
    using ECommerce.Models.ViewModels;
using ECommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace ECommerceWeb.Areas.Customer.Controllers
    {
        [Area("Customer")]
        [Authorize]
        public class CartController : Controller
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailSender _emailSender;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
            public CartController(
                IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager, IEmailSender emailSender, IPaymentService paymentService, IConfiguration configuration
                )
            {
                _unitOfWork = unitOfWork;
                _userManager = userManager;
                _emailSender = emailSender;
                _paymentService = paymentService;
                _configuration = configuration;
            }

        
            public async Task<IActionResult> Index()
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);
                CartVM cartVM = new CartVM();

                if (cart == null)
                {
                    return View(cartVM);
                }

                var cartItems = _unitOfWork.CartItem
                    .GetAll()
                    .Where(c => c.CartId == cart.Id)
                    .ToList();

                var products = _unitOfWork.Product
                    .GetAll()
                    .ToList();

                foreach (var item in cartItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product == null)
                        continue;

                    cartVM.CartItems.Add(new CartItemVM
                    {
                        CartItemId = item.Id,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }

                cartVM.Total = cartVM.CartItems.Sum(c => c.Total);
                return View(cartVM);
            }
            [HttpPost]
            public async Task<IActionResult> AddToCart(int ProductId) {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) { 
                    return NotFound();
                }
                var Product = _unitOfWork.Product.Get(p => p.Id == ProductId);
                if(Product == null)
                    return NotFound();

            if (Product.StockQuantity <= 0)
            {
                TempData["Error"] = "This product is out of stock.";
                return RedirectToAction("Index", "Product");
            }

            var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);

                if (cart == null) { 
                    cart = new Cart
                    {
                        UserId = user.Id,
                    };
                    _unitOfWork.Cart.Add(cart);
                    _unitOfWork.Save();
                }
                var cartItem = _unitOfWork.CartItem.Get(ci => ci.CartId == cart.Id && ci.ProductId == ProductId);

                if (cartItem == null) 
                { 
                    cartItem = new CartItem

                    {
                        CartId = cart.Id,
                        ProductId = ProductId,
                        Quantity = 1,
                        Price = Product.Price
                    };

                    _unitOfWork.CartItem.Add(cartItem);
                }
                else
                {
                    if (cartItem.Quantity >= Product.StockQuantity){
                        TempData["Error"] = "You cannot add more than the available stock.";
                        return RedirectToAction("Index", "Product");
                    }
                    cartItem.Quantity ++;
                    _unitOfWork.CartItem.Update(cartItem);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }

            [HttpPost]
            public async Task<IActionResult> Increase(int cartItemId)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();
            
                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);
                if(cart == null)
                    return NotFound();

                var cartItem = _unitOfWork.CartItem.Get(ci => ci.Id == cartItemId && ci.CartId == cart.Id);
                if(cartItem == null)
                    return NotFound();


                var product = _unitOfWork.Product.Get(p => p.Id == cartItem.ProductId);
                if (product == null) 
                   return NotFound();

                if(cartItem.Quantity >= product.StockQuantity)
                {
                    TempData["Error"] = "You cannot add more than the available stock.";
                    return RedirectToAction("Index");
                }
                cartItem.Quantity++;
            
                _unitOfWork.CartItem.Update(cartItem);
                _unitOfWork.Save();
            
                return RedirectToAction("Index");

            }

            [HttpPost]
            public async Task<IActionResult> Decrease(int cartItemId)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();

                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);
                if (cart == null)
                    return NotFound();

                var cartItem = _unitOfWork.CartItem.Get(
                    c => c.Id == cartItemId && c.CartId == cart.Id);
                if (cartItem == null)
                    return NotFound();

                if (cartItem.Quantity > 1)
                { 
                    cartItem.Quantity--;
                    _unitOfWork.CartItem.Update(cartItem);
                    _unitOfWork.Save();
                }
            
                return RedirectToAction("Index");

            }

            [HttpPost]
            public async Task<IActionResult> Remove(int cartItemId)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();

                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);
                if (cart == null)
                    return NotFound();

                var cartItem = _unitOfWork.CartItem.Get(
                    c => c.Id == cartItemId && c.CartId == cart.Id);
                if (cartItem == null)
                    return NotFound();

                _unitOfWork.CartItem.Remove(cartItem);
                _unitOfWork.Save();

                return RedirectToAction("Index");

            }

            [HttpGet]
            public async Task<IActionResult> Checkout()
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);

                if (cart == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var cartItems = _unitOfWork.CartItem
                    .GetAll()
                    .Where(c => c.CartId == cart.Id)
                    .ToList();

                if (cartItems.Count == 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                var products = _unitOfWork.Product
                    .GetAll()
                    .ToList();

                decimal total = 0;

                foreach (var item in cartItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product != null)
                    {
                        total += product.Price * item.Quantity;
                    }
                }

                CheckOutVM checkoutVM = new CheckOutVM
                {
                    Name = user.FullName ?? "",
                    Address = user.Address ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Total = total
                };

                return View(checkoutVM);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Checkout(CheckOutVM checkoutVM)
            {
                if (!ModelState.IsValid)
                {
                    return View(checkoutVM);
                }

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var cart = _unitOfWork.Cart.Get(c => c.UserId == user.Id);

                if (cart == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var cartItems = _unitOfWork.CartItem
                    .GetAll()
                    .Where(c => c.CartId == cart.Id)
                    .ToList();

                if (!cartItems.Any())
                {
                    return RedirectToAction(nameof(Index));
                }

                var products = _unitOfWork.Product
                    .GetAll()
                    .ToList();

                Order order = new Order
                {
                    UserId = user.Id,
                    Name = checkoutVM.Name,
                    Address = checkoutVM.Address,
                    PhoneNumber = checkoutVM.PhoneNumber,
                    OrderDate = DateTime.Now
                };

                decimal total = 0;

                _unitOfWork.Order.Add(order);
                _unitOfWork.Save();
            

                foreach (var cartItem in cartItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                    if (product == null)
                        continue;

                    if (cartItem.Quantity > product.StockQuantity){
                        TempData["Error"] = $"{product.Name} does not have enough stock.";

                        return RedirectToAction(nameof(Index));
                    }

                    OrderItem orderItem = new(){
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Product = product,
                            Quantity = cartItem.Quantity,
                            Price = product.Price

                    };


                    total += product.Price * cartItem.Quantity;

                    _unitOfWork.OrderItem.Add(orderItem);
                }

                order.Total = total;

                _unitOfWork.Order.Update(order);

                _unitOfWork.Save();

                var orderItems = _unitOfWork.OrderItem
                    .GetAll()
                    .Where(oi => oi.OrderId == order.Id)
                    .ToList();

                foreach (var item in orderItems)
                {
                    item.Product = products.First(p => p.Id == item.ProductId);
                }

                var paymentUrl = await _paymentService.CreateCheckoutSessionAsync(order,orderItems);

                return Redirect(paymentUrl);
            }

            [HttpGet]
            public async Task<IActionResult> PaymentSuccess(string session_id)
            {
                if (string.IsNullOrEmpty(session_id))
                    return BadRequest();

                StripeConfiguration.ApiKey =_configuration["Stripe:SecretKey"];

                var service = new SessionService();
                var session = await service.GetAsync(session_id);

                if (session.PaymentStatus != "paid")
                    return BadRequest("Payment was not completed.");

                var order = _unitOfWork.Order.Get(
                    o => o.Id == int.Parse(session.Metadata["OrderId"])
                );

                if (order == null)
                    return NotFound();

                if (order.IsPaid)
                    return RedirectToAction(nameof(Index));

                order.IsPaid = true;
                order.PaymentTransactionId = session.Id;

                var orderItems = _unitOfWork.OrderItem
                    .GetAll()
                    .Where(oi => oi.OrderId == order.Id)
                    .ToList();

                foreach (var item in orderItems)
                {
                    var product = _unitOfWork.Product.Get(
                        p => p.Id == item.ProductId
                    );

                    if (product == null)
                        continue;

                    product.StockQuantity -= item.Quantity;

                    _unitOfWork.Product.Update(product);
                }

                _unitOfWork.Order.Update(order);
                _unitOfWork.Save();

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var cart = _unitOfWork.Cart.Get(
                        c => c.UserId == user.Id
                    );

                    if (cart != null)
                    {
                        var cartItems = _unitOfWork.CartItem
                            .GetAll()
                            .Where(c => c.CartId == cart.Id)
                            .ToList();

                        foreach (var cartItem in cartItems)
                        {
                            _unitOfWork.CartItem.Remove(cartItem);
                        }

                        _unitOfWork.Save();
                    }

                    await _emailSender.SendEmailAsync(
                        user.Email!,
                            $"Payment Successful - Order #{order.Id}",
                            $"""
                            <h1>Payment Successful!</h1>

                            <p>Hi {order.Name},</p>

                            <p>Your payment has been successfully received.</p>

                            <p>
                                <strong>Order:</strong> #{order.Id}<br>
                                <strong>Total:</strong> ${order.Total:N2}<br>
                                <strong>Payment:</strong> Paid
                            </p>

                            <p>Thank you for shopping with us!</p>
                            """
                    );
                }

                return RedirectToAction(nameof(OrderSuccess), new { id = order.Id });
            }
            [HttpGet]
            public IActionResult PaymentCancel()
            {
                return RedirectToAction(nameof(Index));
            }
            [HttpGet]
            public IActionResult OrderSuccess(int id)
            {
                return View(id);
            }

    }
    }
