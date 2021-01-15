using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using ShoeStore.Models;
using ShoeStore.Repositories;
using ShoeStore.Services;

namespace ShoeStore.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext _applicationDB;
        private ShoeStoreDbContext _storeDB;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string userID { get; set; }
        public IEnumerable<Shoe> shoeQuery { get; set; }

        public CartController(ApplicationDbContext applicationDB, ShoeStoreDbContext storeDB,
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDB = applicationDB;
            _storeDB = storeDB;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public string SignInUser()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);

            userID = _userManager.GetUserId(User);
            cookieHelper.Set("UserID", userID, 1);

            userID = cookieHelper.Get("UserID");

            return userID;
        }

        public IActionResult AddToCart(int id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);

            if (!_signInManager.IsSignedIn(User))
            {
                cookieHelper.Remove("UserID");
                return Redirect("/Identity/Account/Login");
            }

            SignInUser();

            var _query = (from p in _storeDB.UserCart
                         where p.ShoeId == id && p.CartId == id
                         select p).FirstOrDefault();

            if (_query != null) //if item is already in cart...need update quantity logic
            {
                var shoeQuery = new CartRepo(_storeDB).UpdateCart(id, userID);
                return View(shoeQuery);
            }
            var query = new CartRepo(_storeDB).AddToCart(id, userID);
            return View(query);
        }

        public IActionResult ViewCart()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                 Response);

            if (!_signInManager.IsSignedIn(User))
            {
                cookieHelper.Remove("UserID");
                return Redirect("/Identity/Account/Login");
            }

            SignInUser();

            var query = new CartRepo(_storeDB).GetAllShoesFromCart(userID);
            return View("AddToCart", query);
        }

        public IActionResult Details(int? id)
        {
            return View(_storeDB.Shoe.Where(p => p.ShoeId == id).FirstOrDefault());
        }

        public IActionResult Delete(int? id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);

            userID = cookieHelper.Get("UserID");

            var query = (from p in _storeDB.UserCart
                         where p.ShoeId == id && p.Id == userID
                         select p).FirstOrDefault();

            if (query != null)
            {
                _storeDB.UserCart.Remove(query);
                _storeDB.SaveChanges();
            }

            return View(query);
        }
    }
}