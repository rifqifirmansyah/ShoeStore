using ShoeStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShoeStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShoeStoreDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Shoe.Any())
            {
                return;   // DB has been seeded
            }

            var shoes = new Shoe[]
            {
                new Shoe
                {
                    ShoeId = 1,
                    ShoeName = "Air Jordan Retro 1",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe1.jpg"),
                    Price = 299.90m
                },
                new Shoe
                {
                    ShoeId = 2,
                    ShoeName = "Air Jordan Retro 2",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe2.jpg"),
                    Price = 299.90m
                },
                new Shoe
                {
                    ShoeId = 3,
                    ShoeName = "Air Jordan Retro 3",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe3.jpg"),
                    Price = 299.90m
                },
                new Shoe
                {
                    ShoeId = 4,
                    ShoeName = "Air Jordan Retro 4",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe4.jpg"),
                    Price = 299.90m
                },
                new Shoe
                {
                    ShoeId = 5,
                    ShoeName = "Air Jordan Retro 5",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe5.jpg"),
                    Price = 299.90m
                },
                new Shoe
                {
                    ShoeId = 6,
                    ShoeName = "Air Jordan Retro 6",
                    ShoeImage = System.IO.File.ReadAllBytes(@"C:\shoe6.jpg"),
                    Price = 299.90m
                }
            };

            foreach (Shoe item in shoes)
            {
                context.Shoe.Add(item);
            }

            context.SaveChanges();
        }
    }
}
