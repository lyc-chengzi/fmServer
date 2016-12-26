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
    public class DialogController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/Dialog/";
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        
        //显示银行列表
        public ActionResult ShowBankList()
        {
            return View(viewFolder + "ShowBankList.cshtml", db.Banks.ToList());
        }

        //显示用户银行账号列表
        public ActionResult ShowBankNoList()
        {
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;

            return View(viewFolder + "ShowBankNoList.cshtml", db.UserBanks.Where(u => u.UserID == loginUser.ID).OrderBy(c=>c.BankID).ToList());
        }

        //登录页
        public ActionResult Login()
        {
            return View(viewFolder + "Login.cshtml");
        }

        //显示费用项目列表
        public ActionResult ShowFeeItemList()
        {
            return View(viewFolder + "ShowFeeItemList.cshtml", WebComm.GetFeeItemListByXml());
        }

        //显示费用项目列表
        public ActionResult ShowAccountTypeList()
        {
            return View(viewFolder + "ShowAccountTypeList.cshtml", WebComm.GetAccountListByXml());
        }

        //显示用户列表
        public ActionResult ShowUserList() 
        {
            return View(viewFolder + "ShowUserList.cshtml", db.Users.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}