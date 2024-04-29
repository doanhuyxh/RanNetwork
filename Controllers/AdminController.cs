using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RanNetwork.Data;
using RanNetwork.Models;

namespace RanNetwork.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("admin_ql")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("admin_ql/user")]
        public IActionResult User()
        {
            return View();
        }
        [HttpGet("admin_ql/webconfig")]
        public IActionResult WebConfig()
        {
            ViewBag.Youtube = _context.WebConfig.FirstOrDefault(i => i.KeyName == "Youtube")?.value ?? "";
            ViewBag.FileGame = _context.WebConfig.FirstOrDefault(i => i.KeyName == "FileGame")?.value ?? "";
            return View();
        }

        [HttpPost("admin_ql/save_link")]
        public IActionResult SaveLink([FromForm] string link, [FromQuery] string type)
        {
            WebConfig linkWebConfig;

            switch (type)
            {
                case ("Youtube"):
                    linkWebConfig = _context.WebConfig.FirstOrDefault(i => i.KeyName == "Youtube");
                    if (linkWebConfig != null)
                    {
                        linkWebConfig.value = link;
                        _context.Update(linkWebConfig);
                        _context.SaveChanges();
                    }
                    break;

                case ("FileGame"):
                    linkWebConfig = _context.WebConfig.FirstOrDefault(i => i.KeyName == "FileGame");
                    if (linkWebConfig != null)
                    {
                        linkWebConfig.value = link;
                        _context.Update(linkWebConfig);
                        _context.SaveChanges();
                    }
                    break;

            }


            return Ok(new
            {
                code = 200,
                message = "success"
            });
        }


        [DisableRequestSizeLimit]
        [HttpPost("admin_ql/upload_file")]
        public async Task<IActionResult> UploadChunk(IFormFile file, long offset, string filename)
        {
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "upload", filename);

                using (var stream = new FileStream(path, offset == 0 ? FileMode.Create : FileMode.Append))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(Path.Combine("upload", filename));
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}
