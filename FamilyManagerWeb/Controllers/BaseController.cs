using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FamilyManagerWeb.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 电脑端web记账
        /// </summary>
        protected const string APPLY_DATASOURCE_PCWeb = "PCWeb";
        /// <summary>
        /// ios版APP记账
        /// </summary>
        protected const string APPLY_DATASOURCE_IOSAPP = "iosAPP";
    }
}
