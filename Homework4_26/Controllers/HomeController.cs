using Homework4_26.Data;
using Homework4_26.Models;
using Homework4_26.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Homework4_26.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _environment = environment;
        }

        public IActionResult Index()
        {
            IndexViewModel vm = new();
            var imageRepo = new ImageRepo(_connectionString);
            vm.Images = imageRepo.GetAll();
            return View(vm);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            imageFile.CopyTo(stream);
            image.ImageSource = fileName;
            image.DateCreated = DateTime.Now;
            image.Likes = 0; 
            var imageRepo = new ImageRepo(_connectionString);
            imageRepo.Add(image);
            return Redirect("/home/index");
        }

        public IActionResult ViewImage(int id)
        {
            //get cookie here if already like, disable
            //pass a bool through vm if disabled, and then disable if true
            var imageRepo = new ImageRepo(_connectionString);
            var img = imageRepo.GetById(id);
            ViewImageViewModel vm = new()
            {
                Image = img,
            };
            if (HttpContext.Session.GetString("likedids") != null)
            {
                var likedIds = HttpContext.Session.Get<List<int>>("likedids");
                vm.AlreadyHere = likedIds.Any(i => i == id);

            }
            else
            {
                vm.AlreadyHere = false;
            }
            return View(vm);
        }

        [HttpPost]
        public void AddLike(int id)
        {            
            var imageRepo = new ImageRepo(_connectionString);
            imageRepo.AddLike(id);
            //create cookie here

            List<int> likedIds = HttpContext.Session.Get<List<int>>("likedids") ?? new List<int>();

            likedIds.Add(id);

            HttpContext.Session.Set("likedids", likedIds);

        }

        public IActionResult GetLikes(int id)
        {
            var imageRepo = new ImageRepo(_connectionString);
            return Json(new { Likes = imageRepo.GetLikes(id) });
        }


      
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}