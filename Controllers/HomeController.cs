using Microsoft.AspNetCore.Mvc;
using RanNetwork.Data;
using RanNetwork.Models;
using System.Diagnostics;

namespace RanNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Youtube = _context.WebConfig.FirstOrDefault(i => i.KeyName == "Youtube")?.value ?? "";
            ViewBag.FileGame = _context.WebConfig.FirstOrDefault(i => i.KeyName == "FileGame")?.value ?? "";
            return View();
        }

        [HttpGet("dapp/downloads")]
        public IActionResult DappDownloads()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
