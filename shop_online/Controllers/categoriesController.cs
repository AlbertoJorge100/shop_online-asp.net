using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shop_online.Controllers
{
    public class categoriesController : Controller
    {
        // GET: categories
        public ActionResult Index()
        {
            return View();
        }
    }
}