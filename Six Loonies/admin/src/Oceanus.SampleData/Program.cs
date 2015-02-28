using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oceanus.Data;

namespace Oceanus.SampleData
{
    public class Program
    {
        private static OceanusEntities _db = new OceanusEntities();

        static void Main(string[] args)
        {
            List<Category> cats = new List<Category>()
            {
                new Category(){ Name = "Home Improvement" },
                new Category(){ Name = "Recreation" }
            };

            foreach (var cat in cats)
            {
                if (!_db.Categories.Any(c => c.Name.Equals(cat.Name)))
                {
                    _db.AddToCategories(cat);
                }
            }

            _db.SaveChanges();

            Console.ReadLine();
        }
    }
}
