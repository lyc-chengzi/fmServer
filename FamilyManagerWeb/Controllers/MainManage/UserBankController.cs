using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;

namespace FamilyManagerWeb.Controllers
{
    public class UserBankController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/MainManage/UserBank/";
        private const int currentPage = 1;
        private const int pageSize = 20;
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();


        //用户开户银行列表
        public ActionResult List()
        {
            BindBankCardType();
            BindComboxUser();
            List<UserBank> list = GetUserBankList(1, null);
            return View(viewFolder + "List.cshtml", list);
        }

        //用户开户银行列表
        [HttpPost]
        public ActionResult List(UserBank uBank)
        {
            int pageNO = 1;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out pageNO);
            }
            BindBankCardType();
            BindComboxUser();
            List<UserBank> list = GetUserBankList(pageNO, uBank);
            return View(viewFolder + "List.cshtml", list);
        }



        //跳转到添加页
        [ActionName("toCreate")]
        public ActionResult Create()
        {
            //绑定银行卡类型
            ViewBag.CardTypeList = WebComm.GetBankCardType();
            return View(viewFolder + "Create.cshtml");
        }

        //添加用户开户账号
        [HttpPost, ActionName("doCreate")]
        public string Create(UserBank userbank)
        {
            try
            {
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                userbank.BankID = Convert.ToInt32(Request.Form["search_bank.BankID"]);
                userbank.BankName = Request.Form["search_bank.BankName"];
                userbank.UserID = loginUser.ID;
                userbank.UserName = loginUser.cUserName;
                userbank.CreateDate = DateTime.Now;

                db.UserBanks.Add(userbank);
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "添加成功", "userBankList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "添加失败！" + ex.Message, "", "", CallBackType.none, "");
            }

        }

        [ActionName("toEdit")]
        public ActionResult Edit(int id = 0)
        {
            //绑定银行卡类型
            ViewBag.CardTypeList = WebComm.GetBankCardType();
            UserBank userbank = db.UserBanks.Find(id);
            if (userbank == null)
            {
                return RedirectToAction("GoTo404Page", "CommView");
            }
            return View(viewFolder + "Edit.cshtml", userbank);
        }

        //
        // POST: /UserBank/Edit/5

        [HttpPost, ActionName("doEdit")]
        public string Edit(UserBank userbank)
        {

            try
            {
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                userbank.BankID = Convert.ToInt32(Request.Form["search_bank.BankID"]);
                userbank.BankName = Request.Form["search_bank.BankName"];
                userbank.UserID = loginUser.ID;
                userbank.UserName = loginUser.cUserName;

                db.Entry(userbank).State = EntityState.Modified;
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "修改成功", "userBankList", "", CallBackType.none, "");
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
                    UserBank bank = db.UserBanks.Find(item);
                    if (db.Apply_Sub.Where(u => u.UserBankID == bank.ID).Count() > 0 || db.Apply_Sub_CashChange.Where(u => u.InUserBankID == bank.ID || u.OutUserBankID == bank.ID).Count() > 0)
                    {
                        continue;
                    }
                    db.UserBanks.Remove(bank);
                }
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功", "userBankList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "删除失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region 私有方法

        /// <summary>
        /// 获取用户开户行列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="uBank">查询实体</param>
        /// <returns></returns>
        private List<UserBank> GetUserBankList(int currentPage, UserBank uBank)
        {
            var ubList = db.UserBanks.AsQueryable();
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
            //区分用户加载数据，如果不是超级管理员，只能查看自己的数据
            if (!UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                ubList = ubList.Where(ub => ub.UserID == loginUser.ID);
            }

            //筛选卡号
            if (uBank != null && !string.IsNullOrEmpty(uBank.BankNo))
            {
                ubList = ubList.Where(ub => ub.BankNo.Contains(uBank.BankNo));
            }
            //筛选银行名称
            if (uBank != null && !string.IsNullOrEmpty(uBank.BankName))
            {
                ubList = ubList.Where(ub => ub.BankName.Contains(uBank.BankName));
            }
            //筛选银行卡类型
            if (uBank != null && !string.IsNullOrEmpty(uBank.BankCardType) && uBank.BankCardType != "-1")
            {
                ubList = ubList.Where(ub => ub.BankCardType == uBank.BankCardType);
            }
            //筛选用户
            if (uBank != null && !string.IsNullOrEmpty(uBank.UserID.ToString()) && uBank.UserID > 0)
            {
                ubList = ubList.Where(ub => ub.UserID == uBank.UserID);
            }

            SetPagerOptions(ubList.Count(), currentPage);
            List<UserBank> list = ubList.OrderBy(b => b.BankID).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return list;
        }

        /// <summary>
        /// 绑定用户下拉列表
        /// </summary>
        private void BindComboxUser()
        {
            //设置选择用户下拉框html属性
            Dictionary<string, object> attrApplyUserID = new Dictionary<string, object>();
            attrApplyUserID.Add("class", "combox");
            ViewBag.attrApplyUserID = attrApplyUserID;

            //绑定值
            List<SelectListItem> listItem = new List<SelectListItem>();
            SelectListItem liDefault = new SelectListItem();
            liDefault.Text = "全部";
            liDefault.Value = "-1";
            liDefault.Selected = true;
            listItem.Add(liDefault);
            foreach (var item in db.Users.Where(u => u.cUserFlag == true))
            {
                SelectListItem li = new SelectListItem();
                li.Text = item.cUserName;
                li.Value = item.ID.ToString();
                listItem.Add(li);
            }
            ViewBag.listItem = listItem;
        }

        /// <summary>
        /// 绑定银行卡类型
        /// </summary>
        private void BindBankCardType()
        {
            Dictionary<string, string> dataSource = new Dictionary<string, string>();
            foreach (var item in WebComm.GetBankCardType())
            {
                dataSource.Add(item, item);
            }
            Dictionary<string, object> attrCardType = null;
            List<SelectListItem> itemList = null;
            WebComm.BindComBox(dataSource, out attrCardType, out itemList);
            ViewBag.attrCardType = attrCardType;
            ViewBag.itemList = itemList;
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