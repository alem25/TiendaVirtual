﻿using Microsoft.AspNet.Identity;
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
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            string[] roleNames = { "Admin", "Comprador" };
            List<IdentityRole> identityRoles = roleManager.Roles.ToList();

            foreach (var rol in roleNames)
            {
                if (!identityRoles.Any(s=>s.Name.Equals(rol)))
                    roleManager.Create(new IdentityRole(rol));
            }

            const string admin = "the@admin";
            const string adminPass = "3edcVFR$";

            if (!userManager.Users.Any(u => u.Email.Equals(admin)))
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                };

                IdentityResult createAdminUser = userManager.Create(adminUser, adminPass);

                if (createAdminUser.Succeeded)
                    userManager.AddToRole(adminUser.Id, roleNames[0]);
            }

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