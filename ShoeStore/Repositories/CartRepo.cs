using ShoeStore.Data;
using ShoeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStore.Repositories
{
    public class CartRepo
    {
        private ShoeStoreDbContext _context;
        public string userID { get; set; }
        public IEnumerable<Shoe> shoeQuery { get; set; }

        public CartRepo(ShoeStoreDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Shoe> AddToCart(int id, string userID)
        {
            UserCart userCart = new UserCart();
            userCart.CartId = id;
            userCart.ShoeId = id;
            userCart.Id = userID;
            userCart.Quantity += 1;

            _context.UserCart.Add(userCart);
            _context.SaveChanges();

            GetAllShoesFromCart(userID);
            return shoeQuery;
        }

        public IEnumerable<Shoe> GetAllShoesFromCart(string userID)
        {
            var userQuery = from p in _context.UserCart
                            where p.Id == userID
                            select p;

            shoeQuery = from p in _context.Shoe
                           from q in userQuery
                           where p.ShoeId == q.ShoeId
                           select p;

            return shoeQuery;
        }

        public IEnumerable<Shoe> UpdateCart(int id, string userID)
        {
            var _query = (from p in _context.UserCart
                          where p.ShoeId == id && p.CartId == id
                          select p).FirstOrDefault();

            _query.Quantity += 1;
            _context.SaveChanges();

            GetAllShoesFromCart(userID);
            return shoeQuery;
        }

        public UserCart getUserCart(int id)
        {
            var query = (from p in _context.UserCart
                          where p.ShoeId == id && p.CartId == id
                          select p).FirstOrDefault();

            return query;
        }
    }
}
