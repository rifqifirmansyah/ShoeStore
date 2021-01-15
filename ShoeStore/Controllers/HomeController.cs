using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class HomeController : Controller
    {
        private ApplicationDbContext _applicationDB;
        private ShoeStoreDbContext _storeDB;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext applicationDB, ShoeStoreDbContext storeDB,
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDB = applicationDB;
            _storeDB = storeDB;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder, UserSearch userSearch, int? page)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            if (_signInManager.IsSignedIn(User))
            {
                string userID = _userManager.GetUserId(User);
                cookieHelper.Set("UserID", userID, 1);
                HttpContext.Session.SetString("UserID", userID);
                ViewBag.userId = HttpContext.Session.GetString("UserID");
            }
            if (!_signInManager.IsSignedIn(User))
            {
                ViewBag.userId = "";
            }
            if (User.Identity.Name != null)
            {
                var userName = User.Identity.Name;
                if (HttpContext.Session.GetString(userName) == null)
                {
                    string name = userName;
                    ViewBag.name = name;
                    HttpContext.Session.SetString("username", name);

                }
                else
                {
                    ViewBag.name = HttpContext.Session.GetString(userName);
                }
            }

            string sort = String.IsNullOrEmpty(sortOrder) ? "title_asc" : sortOrder;
            string search = String.IsNullOrEmpty(userSearch.searchString) ? "" : userSearch.searchString;

            ViewData["CurrentSort"] = sort;
            ViewData["CurrentFilter"] = search;
            int pageSize = 4;

            var query = new ShoeRepo(_storeDB).GetAll(sort, userSearch);
            return View(PaginatedList<Shoe>.Create(query, page ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult About(string sortOrder, UserSearch userSearch, int? page)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            if (_signInManager.IsSignedIn(User))
            {
                string userID = _userManager.GetUserId(User);
                cookieHelper.Set("UserID", userID, 1);
                HttpContext.Session.SetString("UserID", userID);
                ViewBag.userId = HttpContext.Session.GetString("UserID");
            }
            if (!_signInManager.IsSignedIn(User))
            {
                ViewBag.userId = "";
            }
            if (User.Identity.Name != null)
            {
                var userName = User.Identity.Name;
                if (HttpContext.Session.GetString(userName) == null)
                {
                    string name = userName;
                    ViewBag.name = name;
                    HttpContext.Session.SetString("username", name);

                }
                else
                {
                    ViewBag.name = HttpContext.Session.GetString(userName);
                }
            }

            string sort = String.IsNullOrEmpty(sortOrder) ? "title_asc" : sortOrder;
            string search = String.IsNullOrEmpty(userSearch.searchString) ? "" : userSearch.searchString;

            ViewData["CurrentSort"] = sort;
            ViewData["CurrentFilter"] = search;
            int pageSize = 4;

            var query = new ShoeRepo(_storeDB).GetAll(sort, userSearch);
            return View(PaginatedList<Shoe>.Create(query, page ?? 1, pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ClearCookie(string key)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            cookieHelper.Remove(key);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
