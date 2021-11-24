using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vulnerable.MVC.Areas.Identity.Data;
using Vulnerable.MVC.Models;

namespace Vulnerable.MVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserAdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await ListAccounts();
            var model = new UserAccountsViewModel(accounts.Admins, accounts.Users);
            return View(model);
        }

        private async Task<(IList<User> Admins, IList<User> Users)> ListAccounts()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var users = await _userManager.GetUsersInRoleAsync("User");

            return (admins, users);
        }
    }
}
