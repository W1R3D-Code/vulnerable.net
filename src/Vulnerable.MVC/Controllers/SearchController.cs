using Microsoft.AspNetCore.Mvc;
using Vulnerable.Common.Consts;
using Vulnerable.Common.Enums;
using Vulnerable.MVC.Filters;

namespace Vulnerable.MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        
        public SearchController(ILogger<SearchController> logger)
            => _logger = logger;

        [ChallengeFilter("Dom based XSS", ChallengeCategory.Injection, VulnerabilityConsts.DomXssRegex)]
        public IActionResult Index(string q)
        {
            if (string.IsNullOrEmpty(q))
                return RedirectToAction("Index", "Home");

            return View("Index", q);
        }
    }
}
