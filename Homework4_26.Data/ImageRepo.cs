using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4_26.Data
{
    public class ImageRepo
    {
        private string _connectionString;

        public ImageRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetAll()
        {
            using var context = new Homework4_26DbContext(_connectionString);
            return context.Images.ToList();
        }

        public void Add(Image image)
        {
            using var context = new Homework4_26DbContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public Image GetById(int id)
        {
            using var context = new Homework4_26DbContext(_connectionString);
            return context.Images.FirstOrDefault(img => img.Id == id);
        }

        public void AddLike(int id)
        {
            using var context = new Homework4_26DbContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE Images SET likes = likes + 1 WHERE id = {id}");
            context.SaveChanges();
        }

        public int GetLikes(int id)
        {
            using var context = new Homework4_26DbContext(_connectionString);
            return context.Images.FirstOrDefault(img => img.Id == id).Likes;
        }

    }
}
