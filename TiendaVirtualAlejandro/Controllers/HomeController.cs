using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtualAlejandro.Models;

namespace TiendaVirtualAlejandro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ApplicationDbContext context = new ApplicationDbContext();

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //var role = new IdentityRole();
            //role.Name = "Admin";
            //roleManager.Create(role);

            //userManager.AddToRole(User.Identity.GetUserId(), "Admin");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}