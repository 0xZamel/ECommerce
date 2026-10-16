using ECommerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    [Area("Admin")]

    public class TestController : Controller
    {
        private readonly IEmailSender _emailSender;

        public TestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> SendEmail()
        {
            await _emailSender.SendEmailAsync(
                "omar.zamel5@gmail.com",
                "Test Email",
                "<h1>Hello!</h1><p>Email sending works!</p>"
            );

            return Content("Email sent successfully!");
        }
    }
}