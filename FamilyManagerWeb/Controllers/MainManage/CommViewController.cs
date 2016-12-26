using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyManagerWeb.Controllers
{
    public class CommViewController : Controller
    {
        private const string viewFolder = "~/Views/MainManage/CommView/";
        public ActionResult GoTo404Page()
        {
            return View(viewFolder + "GoTo404Page.cshtml");
        }

    }
}
