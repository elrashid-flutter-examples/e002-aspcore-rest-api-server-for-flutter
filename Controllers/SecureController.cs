using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace App2.Controllers
{
    public class SecureController : Controller
    {

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        public async Task Logout()
        {
            await HttpContext.SignOutAsync("cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
