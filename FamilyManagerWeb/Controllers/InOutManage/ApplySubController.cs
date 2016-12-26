using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using BaseFunction;

namespace FamilyManagerWeb.Controllers
{
    public class ApplySubController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/InOutManage/ApplySub/";
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        //记账明细列表页
        public ActionResult List(int mainBillCode)
        {
            var apply_sub = db.Apply_Sub.Where(a => a.ApplyMain_BillCode == mainBillCode);
            return View(viewFolder + "List.cshtml", apply_sub.ToList());
        }

        //弹出备注信息
        public ActionResult ToUpdateAdd(int id)
        {
            ViewBag.cadd = db.Apply_Sub.Find(id).CAdd;
            ViewBag.SubID = id;
            return View(viewFolder + "UpdateAdd.cshtml");
        }

        //弹出备注信息
        [HttpPost, ActionName("doUpdateAdd")]
        public string UpdateAdd(Apply_Sub sub)
        {
            try
            {
                Apply_Sub updateSub = db.Apply_Sub.Find(sub.ID);
                updateSub.CAdd = sub.CAdd;
                
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "修改成功！", "ApplyInfoListNav", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "修改失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        //执行删除操作
        [HttpPost, ActionName("DeleteById")]
        public string DeleteConfirmed(int id)
        {
            try
            {
                string sql = "exec proc_DeleteAccouting " + id.ToString() + " ";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功！", "ApplySubListNav", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "删除失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        //弹出退款确认框
        public ActionResult TKPage(int id)
        {
            Apply_Sub s = db.Apply_Sub.Where(c => c.ID == id).SingleOrDefault();
            if (s != null)
            {
                ViewBag.imoney = s.iMoney;
            }
            else
            {
                ViewBag.imoney = 0;
            }
            ViewBag.SubID = id;
            return View(viewFolder + "ApplySubTK.cshtml");
        }
        [HttpPost, ActionName("doTK")]
        public string DoTKAction(decimal tkmoney, int id)
        {
            string lycResult = "";
            try
            {
                using (FamilyCaiWuDBEntities db2 = new FamilyCaiWuDBEntities())
                {
                    Apply_Sub s = db2.Apply_Sub.Where(c => c.ID == id).SingleOrDefault();
                    if (s == null)
                    {
                        throw new Exception("此记账信息已不存在，无法执行退款操作！");
                    }
                    if (s.InOutType != "out" || s.CashOrBank != 1)
                    {
                        throw new Exception("此记账信息不是【银行支出记账】，无法执行退款操作！");
                    }
                    if (s.UserBankID == null || s.UserBankID.Value == 0)
                    {
                        throw new Exception("此记账信息不是【银行记账】，无法执行退款操作！");
                    }
                    if (tkmoney == s.iMoney)
                    {
                        throw new Exception("退款金额与原记账金额相同，请直接执行删除操作！");
                    }
                    if (tkmoney > s.iMoney)
                    {
                        throw new Exception("退款金额大于原记账金额，无法执行退款操作！");
                    }
                    //1. 修改原记账信息的 备注、记账金额字段
                    decimal newMoney = s.iMoney - tkmoney;
                    s.CAdd = s.CAdd + ";原记账金额：" + s.iMoney + " 元，现在退款：" + tkmoney + " 元，最终记账金额是：" + newMoney + " 元;";
                    s.iMoney = newMoney;
                    //2. 修改对应的银行余额信息
                    UserBank ub = db2.UserBanks.Where(c => c.ID == s.UserBankID.Value).SingleOrDefault();
                    if (ub == null)
                    {
                        throw new Exception("此记账信息中的银行信息已不存在，无法执行退款操作！");
                    }
                    ub.NowMoney = ub.NowMoney + tkmoney;
                    //3. 修改记账主信息的支出合计
                    Apply_Main m = db2.Apply_Main.Where(c => c.ID == s.ApplyMain_BillCode).SingleOrDefault();
                    if (m != null)
                    {
                        m.ApplyOutMoney = m.ApplyOutMoney - tkmoney;
                    }
                    db2.SaveChanges();
                }
                lycResult = WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "退款成功！", "ApplyInfoListNav", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                lycResult = WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "退款失败！" + ex.Message, "", "", CallBackType.none, "");
            }
            return lycResult;
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}