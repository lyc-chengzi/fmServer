using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using FamilyManagerWeb.Models.ViewModels;
using System.Text;
using BaseFunction;

namespace FamilyManagerWeb.Controllers
{
    public class UserPowerController : Controller
    {
        private const string viewFolder = "~/Views/MainManage/UserPower/";
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        private List<UserModelListEntity> umList = new List<UserModelListEntity>();
        [ActionName("List")]
        public ActionResult UserMenuList()
        {
            ViewBag.userID = 0;
            ViewBag.userName = "";
            return View(viewFolder + "List.cshtml", GetList(0));
        }

        [ActionName("List"), HttpPost]
        public ActionResult UserMenuList(int? id)
        {
            int intTemp = 0;
            string userid = Request.Form["search_User.UserID"];
            int.TryParse(userid, out intTemp);
            ViewBag.userID = intTemp;
            ViewBag.userName = Request.Form["search_User.cUserName"];
            return View(viewFolder + "List.cshtml", GetList(intTemp));
        }

        //跳转到添加权限页
        [ActionName("toAddPower")]
        public ActionResult ToAddUserMenuPower()
        {
            ViewBag.UserID = Request.QueryString["userid"];
            int userID = int.Parse(Request.QueryString["userid"]);
            User user = db.Users.Where(u => u.ID == userID).SingleOrDefault();
            ViewBag.cUserName = user.cUserName;
            ViewBag.cUserCode = user.cUserCode.ToString();
            ViewBag.strTree = GetSysModelTree(userID);
            return View(viewFolder + "AddUserMenuPower.cshtml", null);
        }

        [HttpPost, ActionName("addmp")]
        public string AddUserMenuPower()
        {
            try
            {
                int UserID = 0;
                int.TryParse(Request.Form["UserID"], out UserID);
                string sysModelIDs = Request.Form["checkedModelList"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(sysModelIDs);

                List<UserModelPower> removeList = db.UserModelPowers.Where(m => m.userID == UserID).ToList();
                foreach (var item in removeList)
                {
                    db.UserModelPowers.Remove(item);
                }
                foreach (int id in idList)
                {
                    UserModelPower mpAdd = new UserModelPower() { userID = UserID, modelID = id, modelButtonID = 0 };
                    db.UserModelPowers.Add(mpAdd);
                }


                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "配置成功", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "配置失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="UserID">当前登录人ID</param>
        /// <returns></returns>
        public string GetSysModelTree(int UserID)
        {
            List<UserModelListEntity> list = new List<UserModelListEntity>();
            string sql = " select m.*,um.userID from SysModels m " +
                         " left join UserModelPower um on m.ID = um.modelID and um.userID = " + UserID.ToString() +
                         " where m.isFlag = 1 ";
            DataTable dt = LycSQLHelper.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                UserModelListEntity uml = new UserModelListEntity();
                uml.ID = Convert.ToInt32(dr["ID"].ToString());

                uml.SysModelName = dr["SysModelName"].ToString();
                uml.SysModelPhyURL = dr["SysModelPhyURL"].ToString();
                uml.SysModelRouteName = dr["SysModelRouteName"].ToString();
                uml.SysModelController = dr["SysModelController"].ToString();
                uml.SysModelAction = dr["SysModelAction"].ToString();
                uml.SysModelRel = dr["SysModelRel"].ToString();
                uml.SysModelTarget = dr["SysModelTarget"].ToString();
                uml.SysModelUrlPram = dr["SysModelUrlPram"].ToString();
                uml.SysModelClassID = Convert.ToInt32(dr["SysModelClassID"].ToString());
                uml.SysModelLevel = Convert.ToInt32(dr["SysModelLevel"].ToString());
                uml.IsLast = Convert.ToBoolean(dr["IsLast"].ToString());
                uml.IsHaveAction = Convert.ToBoolean(dr["IsHaveAction"].ToString());
                uml.IsFlag = Convert.ToBoolean(dr["IsFlag"].ToString());
                uml.UserID = dr.IsNull("UserID") ? 0 : Convert.ToInt32(dr["UserID"].ToString());
                uml.Checked = !dr.IsNull("UserID") && uml.IsLast == true ? true : false;
                list.Add(uml);
            }
            umList = list;

            string result = GetSysModel(umList.Where(l => l.SysModelClassID == 0).ToList());
            return result;
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        private string GetSysModel(List<UserModelListEntity> DataSource)
        {
            StringBuilder tree = new StringBuilder("");
            tree.Clear();
            foreach (UserModelListEntity item in DataSource)
            {
                tree.Append("<li>");
                if (item.IsLast == false)
                {
                    tree.Append("<a tname = \"").Append(item.SysModelName).Append("\"")
                        .Append(" tvalue = \"").Append(item.ID).Append("\">")
                        .Append(item.SysModelName).Append("</a>");
                    tree.Append("<ul>");
                    tree.Append(GetSysModel(umList.Where(l => l.SysModelClassID == item.ID).ToList()));
                    tree.Append("</ul>");
                }
                else
                {
                    tree.Append("<a tname=\"").Append(item.SysModelName).Append("\"")
                        .Append(" tvalue = \"").Append(item.ID).Append("\"");
                    if (item.Checked == true)
                    {
                        tree.Append(" checked=\"true\"");
                    }

                    tree.Append(" >").Append(item.SysModelName).Append("</a>");
                }
                tree.Append("</li>");
            }

            return tree.ToString();
        }

        private List<UserMenuPowerEntity> GetList(int userid)
        {
            List<UserMenuPowerEntity> list = new List<UserMenuPowerEntity>();
            var ul = db.UserModelPowers.AsQueryable();
            if (userid > 0)
            {
                ul.Where(l => l.userID == userid);
            }
            list = ul.Select(l => new UserMenuPowerEntity { ID = l.ID, UserName = l.User.cUserName, MenuName = l.SysModel.SysModelName }).ToList();
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}