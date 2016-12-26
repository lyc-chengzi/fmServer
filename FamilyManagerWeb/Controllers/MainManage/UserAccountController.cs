using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;

namespace FamilyManagerWeb.Controllers.MainManage
{
    public class UserAccountController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/MainManage/UserAccount/";
        private const int currentPage = 1;
        private const int pageSize = 20;
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        //
        // GET: /UserAccount/

        public ActionResult List()
        {
            BindComboxUser();
            BindAccountType();
            List<UserAccount> list = GetUserAccountList(1, null);
            return View(viewFolder + "List.cshtml", list);
        }

        //用户账户列表
        [HttpPost]
        public ActionResult List(UserAccount ua)
        {
            int pageNO = 1;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out pageNO);
            }
            BindComboxUser();
            BindAccountType();
            List<UserAccount> list = GetUserAccountList(pageNO, ua);
            return View(viewFolder + "List.cshtml", list);
        }

        //跳转到添加页
        [ActionName("toCreate")]
        public ActionResult Create()
        {
            //绑定银行卡类型
            BindAccountType();
            return View(viewFolder + "Create.cshtml");
        }

        //添加用户开户账号
        [HttpPost, ActionName("doCreate")]
        public string Create(UserAccount ua)
        {
            try
            {
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                ua.UserID = loginUser.ID;
                ua.ctypeName = WebComm.GetAccountListByXml().Where(c => c.TypeID == int.Parse(Request.Form["ctypeID"])).SingleOrDefault().TypeName;


                db.UserAccounts.Add(ua);
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "添加成功", "UserAccountList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "添加失败！" + ex.Message, "", "", CallBackType.none, "");
            }

        }

        [ActionName("toEdit")]
        public ActionResult Edit(int id = 0)
        {
            //绑定账户类型
            BindAccountType();
            UserAccount ua = db.UserAccounts.Find(id);
            if (ua == null)
            {
                return RedirectToAction("GoTo404Page", "CommView");
            }
            return View(viewFolder + "Edit.cshtml", ua);
        }

        //
        // POST: /UserBank/Edit/5

        [HttpPost, ActionName("doEdit")]
        public string Edit(UserAccount ua)
        {

            try
            {
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                ua.ctypeName = WebComm.GetAccountListByXml().Where(c => c.TypeID == int.Parse(Request.Form["ctypeID"])).SingleOrDefault().TypeName;

                db.Entry(ua).State = EntityState.Modified;
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "修改成功", "UserAccountList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "修改失败" + ex.Message, "", "", CallBackType.none, "");
            }


        }

        //批量删除
        [HttpPost, ActionName("Delete")]
        public string DeleteConfirmed()
        {
            try
            {
                string ids = Request["ids"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(ids);
                foreach (int item in idList)
                {
                    UserAccount ua = db.UserAccounts.Find(item);
                    if (ua != null)
                    {
                        db.UserAccounts.Remove(ua);
                    }
                }
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功", "UserAccountList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "删除失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        #region 私有方法

        /// <summary>
        /// 获取用户账号列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="uBank">查询实体</param>
        /// <returns></returns>
        private List<UserAccount> GetUserAccountList(int currentPage, UserAccount uAccount)
        {
            var ubList = db.UserAccounts.AsQueryable();
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
            //区分用户加载数据，如果不是超级管理员，只能查看自己的数据
            if (!UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                ubList = ubList.Where(ub => ub.UserID == loginUser.ID);
            }
            if (uAccount != null && uAccount.UserID > 0)
            {
                ubList = ubList.Where(c => c.UserID == uAccount.UserID);
            }

            if (uAccount != null && uAccount.ctypeID > 0)
            {
                ubList = ubList.Where(c => c.ctypeID == uAccount.ctypeID);
            }
            if (uAccount != null && !string.IsNullOrEmpty(uAccount.cname))
            {
                ubList = ubList.Where(c => c.cname.Contains(uAccount.cname));
            }

            SetPagerOptions(ubList.Count(), currentPage);
            List<UserAccount> list = ubList.OrderBy(b => b.ID).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return list;
        }

        /// <summary>
        /// 绑定用户下拉列表
        /// </summary>
        private void BindComboxUser()
        {
            Dictionary<string, string> dataSource = new Dictionary<string, string>();
            foreach (var item in db.Users.Where(c => c.cUserFlag == true).ToList())
            {
                dataSource.Add(item.cUserCode.ToString(), item.cUserName);
            }
            Dictionary<string, object> attrCardType = null;
            List<SelectListItem> itemList = null;
            WebComm.BindComBox(dataSource, out attrCardType, out itemList);
            ViewBag.attruser = attrCardType;

            ViewBag.userList = itemList;
        }

        /// <summary>
        /// 绑定用户账号类型
        /// </summary>
        private void BindAccountType()
        {
            Dictionary<string, string> dataSource = new Dictionary<string, string>();
            foreach (var item in WebComm.GetAccountListByXml())
            {
                dataSource.Add(item.TypeID.ToString(), item.TypeName);
            }
            Dictionary<string, object> attrCardType = null;
            List<SelectListItem> itemList = null;
            WebComm.BindComBox(dataSource, out attrCardType, out itemList);
            ViewBag.attrAccountType = attrCardType;
            ViewBag.accountList = itemList;
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}