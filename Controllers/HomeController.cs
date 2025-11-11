using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata;
using BlogPersonal.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogPersonal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string path = Directory.GetCurrentDirectory();
            List<Article> articulos = new List<Article>();
            string[] directories = Directory.GetFiles(path, "*.txt");
            foreach(var dir in directories)
            {
                var lines = System.IO.File.ReadAllLines(dir);
                int id = int.Parse(lines[0]);
                string title = lines[1];
                DateOnly publishedDate = DateOnly.Parse(lines[2]);
                Article art = new Article
                {
                    Id = id,
                    Title = title,
                    PublishedDate = publishedDate
                };
                articulos.Add(art);
            }
            return View(articulos);
        }

        public IActionResult Detail(int id)
        {
            string path = Directory.GetCurrentDirectory();
            path += @"\"+id + ".txt";
            Trace.WriteLine("monhi: "+path);
            var lines = System.IO.File.ReadAllLines(path);
            ViewData["Id"] = lines[0];
            ViewData["Title"] = lines[1];
            ViewData["Content"] = lines[3];
            ViewData["PublishedDate"] = lines[2];

            return View();
        }

        public IActionResult Privacy()
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
