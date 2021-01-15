using ShoeStore.Data;
using ShoeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStore.Repositories
{
    public class ShoeRepo
    {
        private ShoeStoreDbContext _context;
        public ShoeRepo(ShoeStoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<Shoe> GetAll(string sortOrder, UserSearch userSearch)
        {
            if (!String.IsNullOrEmpty(userSearch.searchString))
            {

                var query = from p in _context.Shoe
                            where (p.ShoeName.Contains(userSearch.searchString))
                            select p;

                return query;

            }
            else
            {
                var query = from p in _context.Shoe
                            select p;

                switch (sortOrder)
                {
                    case "title_asc":
                        query = query.OrderBy(p => p.ShoeName);
                        break;
                    case "title_desc":
                        query = query.OrderByDescending(p => p.ShoeName);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        query = query.OrderBy(p => p.Price);
                        break;
                };
                return query;
            }
        }

        public Shoe CreateShoe(string shoeName, string shoeImage, decimal price)
        {
            Shoe shoe = (from p in _context.Shoe
                         where p.ShoeId == 1
                         select p).FirstOrDefault();

            //string imagePath = @"C:\" + shoeImage;

            //byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            //string base64String = Convert.ToBase64String(imageBytes);
            string image = Convert.ToBase64String(shoe.ShoeImage);

            Shoe newShoe = new Shoe();

            newShoe.ShoeName = shoeName;
            newShoe.ShoeImage = Convert.FromBase64String(image);
            newShoe.Price = price;

            _context.Shoe.Add(newShoe);
            _context.SaveChanges();

            return newShoe;
        }

        public Shoe UpdateShoe(int shoeId, string shoeName, string shoeImage, decimal price)
        {
            Shoe shoe = (from p in _context.Shoe
                               where p.ShoeId == shoeId
                               select p).FirstOrDefault();
            if (shoe != null)
            {
                //string imagePath = @"C:\" + shoeImage;

                //byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                //string base64String = Convert.ToBase64String(imageBytes);

                string image = Convert.ToBase64String(shoe.ShoeImage);

                shoe.ShoeName = shoeName;
                shoe.ShoeImage = Convert.FromBase64String(image);
                shoe.Price = price;

                _context.Shoe.Update(shoe);
                _context.SaveChanges();
            }
            return shoe;
        }

        public Shoe DeleteShoe(int shoeId)
        {
            Shoe shoe = (from p in _context.Shoe
                         where p.ShoeId == shoeId
                         select p).FirstOrDefault();

            if (shoe != null)
            {
                _context.Remove(shoe);
                _context.SaveChanges();
            }
            return shoe;
        }
    }
}
