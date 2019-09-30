using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_3_1251518_1229918.Controllers
{
    public class HomeController : Controller
    {
        //este controlador unicamente se utilizara para el menu principal
        public ActionResult Index()
        {
            return View();
        }

    }
}