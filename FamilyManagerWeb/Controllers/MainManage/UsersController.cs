using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using FamilyManagerWeb.Filters;


namespace FamilyManagerWeb.Controllers
{

    public class UsersController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/MainManage/Users/";
        private const int currentPage = 1;
        private const int pageSize = 20;

        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        //用户列表页
        public ActionResult List()
        {
            List<User> list = GetUserList(1, null);
            return View(viewFolder + "List.cshtml", list);
        }

        //用户列表页
        [HttpPost]
        public ActionResult List(User user)
        {
            //设置分页参数
            int currentPageTemp = currentPage;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out currentPageTemp);
            }

            List<User> list = GetUserList(1, user);
            return View(viewFolder + "List.cshtml", list);
        }

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(viewFolder + "Details.cshtml", user);
        }

        [ActionName("toCreate")]
        public ActionResult Create()
        {
            //绑定角色
            var roleList = db.Roles.ToList();
            ViewBag.RoleList = roleList;
            return View(viewFolder + "Create.cshtml");
        }

        [HttpPost, ActionName("doCreate")]
        public string CreateUser(User user)
        {
            int newUserCode = db.Users.Max(u => u.cUserCode);
            newUserCode++;
            user.cUserCode = newUserCode;
            try
            {
                if (ModelState.IsValid)
                {
                    user.cUserFlag = true;
                    user.dCreateDate = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, ex.Message + ex.InnerException, "", "", CallBackType.none, "");
            }
            return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "保存成功", "userList", "", CallBackType.none, "");
        }

        [ActionName("toEdit")]
        public ActionResult Edit(int id = 0)
        {
            //绑定角色
            var roleList = db.Roles.ToList();
            ViewBag.RoleList = roleList;


            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(viewFolder + "Edit.cshtml", user);
        }

        [HttpPost, ActionName("doEdit")]
        public string EditUser(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "用户更新失败！" + ex.Message, "", "", CallBackType.none, "");
            }

            return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "用户信息已更新！", "userList", "", CallBackType.closeCurrent, "");
        }

        [HttpPost, ActionName("DeleteByIds")]
        public string DeleteConfirmed()
        {
            try
            {
                string ids = Request["ids"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(ids);
                foreach (var item in idList)
                {
                    User user = db.Users.Find(item);
                    db.Users.Remove(user);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "用户删除失败," + ex.Message, "", "", CallBackType.none, "");
            }

            return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "用户信息已删除", "", "", CallBackType.none, "");
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region 私有方法

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="user">筛选条件</param>
        /// <returns></returns>
        private List<User> GetUserList(int currentPage, User user)
        {
            List<User> list = new List<Models.User>();
            var userList = db.Users.Where(u => u.cUserFlag == true).AsQueryable();
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
            //如果不是超级管理员登录，则只读取当前用户信息
            if (!UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                userList = userList.Where(u => u.cUserCode == loginUser.cUserCode);
            }
            #region 条件筛选
            if (user != null)
            {
                //根据查询条件筛选
                if (user.cUserCode.ToString() != "0")
                {
                    userList = userList.Where(u => u.cUserCode == user.cUserCode);
                }
                if (!string.IsNullOrEmpty(user.cUserName))
                {
                    userList = userList.Where(u => u.cUserName.Contains(user.cUserName));
                }
                if (Request.Form["dCreateDate"] != null && Request.Form["dCreateDate"] != "")
                {
                    userList = userList.Where(u => u.dCreateDate == user.dCreateDate);
                }
            }
            #endregion



            //设置分页
            SetPagerOptions(db.Users.Count(), currentPage);

            list = userList.OrderBy(u => u.ID).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return list;
        }

        /// <summary>
        /// 设置分页参数
        /// </summary>
        private void SetPagerOptions(int recordNo, int CurrentPageNo = 1, int cPageSize = pageSize, int pageNumShown = 10)
        {
            ViewBag.recordNo = recordNo;
            ViewBag.CurrentPageNo = CurrentPageNo;
            ViewBag.pageSize = cPageSize;
            ViewBag.pageNumShown = pageNumShown;
        }

        #endregion
    }
}