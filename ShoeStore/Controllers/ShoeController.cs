using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Controllers
{
    public class ShoeController : Controller
    {
        private ShoeStoreDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string userID { get; set; }
        public IEnumerable<Shoe> shoeQuery { get; set; }

        public ShoeController(ShoeStoreDbContext context, SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return Redirect("/Home/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            return View();
        }

        public IActionResult Details(int? id)
        {
            return View(_context.Shoe.Where(p => p.ShoeId == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            return View(_context.Shoe.Where(p => p.ShoeId == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            return View(_context.Shoe.Where(p => p.ShoeId == id).FirstOrDefault());
        }

        public IActionResult CreateShoe(string shoeName, string shoeImage, decimal price)
        {
            var query = new ShoeRepo(_context).CreateShoe(shoeName, shoeImage, price);
            return View(query);
        }

        public IActionResult EditShoe(int shoeId, string shoeName, string shoeImage, decimal price)
        {
            var query = new ShoeRepo(_context).UpdateShoe(shoeId, shoeName, shoeImage, price);
            return View(query);
        }

        public IActionResult DeleteShoe(int id)
        {
            var query = new ShoeRepo(_context).DeleteShoe(id);
            return View(query);
        }
    }
}