using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using FamilyManagerWeb.Models.ViewModels;
using FamilyManagerWeb.Models;
using System.Text;

namespace FamilyManagerWeb.Controllers
{
    public class UserInfoAPIController : LycMVCController
    {
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        //
        // GET: /UserInfoAPI/

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" > UserLogin(int usercode, string userpwd) <br/>");
            return Content(sb.ToString());
        }

        /// <summary>
        /// 登陆请求
        /// </summary>
        /// <param name="usercode">账号</param>
        /// <param name="userpwd">密码</param>
        /// <returns></returns>
        public JsonResult UserLogin(int usercode, string userpwd)
        {
            LycJsonResult lycResult = new LycJsonResult();

            try
            {
                var user = db.Users.Where(c => c.cUserCode == usercode && c.cUserPwd == userpwd && c.cUserFlag == true)
                    .Select(c => new { ID = c.ID, cUserCode = c.cUserCode, cUserName = c.cUserName })
                    .SingleOrDefault();
                if (user != null)
                {
                    lycResult.Data = new JsonResultModel { bSuccess = true, message = "登陆成功", jsonObj = user };
                }
                else
                {
                    lycResult.Data = new JsonResultModel { bSuccess = false, message = "账号或密码不存在", jsonObj = null };
                }
            }
            catch
            {
                lycResult.Data = new JsonResultModel { bSuccess = false, message = "登陆异常，请稍后再试", jsonObj = null };
            }
            return lycResult;
        }

    }
}
