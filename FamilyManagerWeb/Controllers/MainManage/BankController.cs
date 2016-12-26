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
    public class BankController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/MainManage/Bank/";
        private const int currentPage = 1;
        private const int pageSize = 20;
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        //列表页
        public ActionResult List()
        {
            List<Bank> bankList = GetBankList(1, null);
            return View(viewFolder + "List.cshtml", bankList);
        }

        //列表页查询
        [HttpPost]
        public ActionResult List(Bank bank)
        {
            int pageNO = 1;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out pageNO);
            }
            List<Bank> bankList = GetBankList(pageNO, bank);
            return View(viewFolder + "List.cshtml", bankList);
        }

        //弹出新增页
        [ActionName("toCreate")]
        public ActionResult Create()
        {
            return View(viewFolder + "Create.cshtml");
        }

        //新增方法
        [HttpPost, ActionName("doCreate")]
        public string Create(Bank bank)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Banks.Add(bank);
                    db.SaveChanges();

                    return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "添加银行成功！", "bankMainList", "", CallBackType.none, "");
                }
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "验证失败！" + ex.Message, "", "", CallBackType.none, "");
            }

            return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "验证失败！", "", "", CallBackType.none, "");


        }

        //删除方法
        [HttpPost, ActionName("DeleteByIds")]
        public string DeleteConfirmed()
        {
            try
            {
                string ids = Request["ids"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(ids);
                foreach (int item in idList)
                {
                    Bank bank = db.Banks.Find(item);
                    db.Banks.Remove(bank);
                }
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功", "bankMainList", "", CallBackType.none, "");
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
        /// 根据当前页码，获取银行列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        private List<Bank> GetBankList(int currentPage, Bank bank)
        {
            var bankList = db.Banks.AsQueryable();
            if (bank != null)
            {
                if (!string.IsNullOrEmpty(bank.cBankName))
                {
                    bankList = bankList.Where(b => b.cBankName.Contains(bank.cBankName));
                }                
            }

            List<Bank> list = bankList.OrderBy(b => b.ID).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();


            SetPagerOptions(db.Banks.Count(), currentPage);
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