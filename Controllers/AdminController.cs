using BlogPersonal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using System.IO;

namespace BlogPersonal.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            string path = @"C:\Users\augus\source\repos\BlogPersonal\";
            Article art = new Article();
            List<Article> _articles = new List<Article>();

            string[] directories = Directory.GetFiles(path, "*.txt");
            foreach (var subdirectory in directories)
            {
                var lineas = System.IO.File.ReadAllLines(subdirectory);
                string id = lineas[0];
                string title = lineas[1];
                _articles.Add(new Article
                {
                    Id = int.Parse(id),
                    Title = title
                });

            }
            
            return View(_articles);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New([Bind("Title, PublishedDate,Content")]Article article)
        {
            string path = @"C:\Users\augus\source\repos\BlogPersonal\";
            string[] directories = Directory.GetFiles(path, "*.txt");
            int lastId = 0;
            foreach(var dir in directories){
                var lineas = System.IO.File.ReadAllLines(dir);
                int id = int.Parse(lineas[0]);
                if (id > lastId)
                {
                    lastId = id;
                }
            }
            Article art = new Article
            {
                Id = lastId+1,
                Title = article.Title,
                PublishedDate = article.PublishedDate,
                Content = article.Content
            };
            path +=  art.Id + ".txt";
            if (!System.IO.File.Exists(path))
            {
                using StreamWriter sw = System.IO.File.CreateText(path);
                sw.WriteLine(art.Id);
                sw.WriteLine(art.Title);
                sw.WriteLine(art.PublishedDate);
                sw.WriteLine(art.Content);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            string path = @"C:\Users\augus\source\repos\BlogPersonal\" + id + ".txt";
            if (System.IO.File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                Article art = new Article
                {
                    Id = int.Parse(lines[0]),
                    Title = lines[1],
                    PublishedDate = DateOnly.Parse(lines[2]),
                    Content = lines[3]
                };
                ViewData["Id"] = art.Id;
                ViewData["Title"] = art.Title;
                ViewData["PublishedDate"] = art.PublishedDate;
                ViewData["Content"] = art.Content;
            }
            else
            {
                ViewData["Error"] = "Article not found.";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit([Bind("Id,Title,PublishedDate,Content")] Article article)
        {
            Article art = new Article
            {
                Id = article.Id,
                Title = article.Title,
                PublishedDate = article.PublishedDate,
                Content = article.Content
            };
            string path = @"C:\Users\augus\source\repos\BlogPersonal\" + art.Id + ".txt";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                using StreamWriter sw = System.IO.File.CreateText(path);
                sw.WriteLine(art.Id);
                sw.WriteLine(art.Title);
                sw.WriteLine(art.PublishedDate);
                sw.WriteLine(art.Content);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string path = @"C:\Users\augus\source\repos\BlogPersonal\"+id+".txt";
            System.IO.File.Delete(path);
            return RedirectToAction("Index");
        }
    }
}
