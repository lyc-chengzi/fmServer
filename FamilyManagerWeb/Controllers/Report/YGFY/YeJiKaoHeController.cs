using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyManagerWeb.Controllers.Report.YGFY
{
    public class YeJiKaoHeController : Controller
    {
        //
        // GET: /YeJiKaoHe/

        public ActionResult Index()
        {
            return View("~/Views/Report/YGFY/YeJiKaoHe/Index.cshtml");
        }

        public string Test()
        {
            return "当你看到这句话时，说明你的测试成功了！！！！祝贺";
        }

    }
}
