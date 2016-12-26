using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

using FamilyManagerWeb.Controllers;
using FamilyManagerWeb.Models;
using Newtonsoft.Json;

namespace FamilyManage.WebService
{
    /// <summary>
    /// UserService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/users/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string UserLogin(int userCode, string userPwd)
        {         
            string jsonResult = "";
            FamilyManagerWeb.Models.User user = db.Users.Where(c => c.cUserCode == userCode && c.cUserPwd == userPwd).SingleOrDefault();
            string jsonObj = "";
            
            if (user != null && user.cUserFlag==true)
            {
                jsonObj = "{\"userID\":"+user.ID+",\"userCode\":\""+user.cUserCode+"\",\"userName\":\""+user.cUserName+"\",\"userPwd\":\""+userPwd+"\"}";
                jsonResult = WebComm.ReturnJsonForExterior(true, "登陆成功！", jsonObj);
            }
            else if (user == null)
            {
                jsonResult = WebComm.ReturnJsonForExterior(false, "用户名或密码错误！", null);
            }
            else
            {
                jsonResult = WebComm.ReturnJsonForExterior(false, "用户已无效！", null);
            }
            return jsonResult;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();   
            base.Dispose(disposing);            
        }
    }
}
