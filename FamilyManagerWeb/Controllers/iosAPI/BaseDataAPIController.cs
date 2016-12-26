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
    public class BaseDataAPIController : LycMVCController
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" > GetUpdateTime <br/>");
            sb.AppendLine(" > GetFeeItemList <br/>");
            sb.AppendLine(" > GetFlowTypeList <br/>");
            sb.AppendLine(" > GetUserBankList(int userID) <br/>");
            sb.AppendLine(" > GetALLBanks <br/>");
            return Content(sb.ToString());

        }

        /// <summary>
        /// 获取基础数据更新时间
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUpdateTime()
        {
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                ConfigXml cx = new ConfigXml();
                lycResult.Data = new JsonResultModel(true, "获取基础数据更新时间成功", new { updateDate = cx.BaseDataUpdateTime });
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "获取基础数据更新时间失败", null);
            }
            return lycResult;
        }

        //[HttpPost]
        /// <summary>
        /// 获取所有费用科目
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFeeItemList()
        {
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                lycResult.Data = new JsonResultModel(true, "获取费用科目成功", WebComm.GetFeeItemListByXml());
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "获取费用科目失败", null);
            }
            return lycResult;
        }

        /// <summary>
        /// 获取所有资金流动类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFlowTypeList()
        {
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                lycResult.Data = new JsonResultModel(true, "获取资金类型成功", WebComm.GetFundFlowTypeList());
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "获取资金类型失败", null);
            }
            return lycResult;
        }

        /// <summary>
        /// 获取用户关联的所有银行账户
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserBankList(int userID)
        {
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                var list = (from lu in db.UserBanks
                           where lu.UserID == userID
                           select new 
                           {
                               userBankID = lu.ID,
                               bankID = lu.BankID,
                               bankName = lu.BankName,
                               bankType = lu.BankCardType,
                               money = lu.NowMoney.Value,
                               cardNo = lu.BankNo
                           }).ToList();
                /*
                //foreach (var item in list)
                //{
                //    item.money = item.dmoney.Value.ToString();
                //}
                List<DTOUserBank> lub = new List<DTOUserBank>();
                for (int i = 0; i < list.Count; i++)
                {
                    var source = list[i];
                    DTOUserBank ub = new DTOUserBank();
                    ub.userBankID = source.userBankID;
                    ub.bankID = source.bankID;
                    ub.bankName = source.bankName;
                    ub.bankType = source.bankType;
                    ub.money = source.money.ToString();
                    ub.cardNo = source.cardNo;
                    lub.Add(ub);
                }*/
                lycResult.Data = new JsonResultModel(true, "获取银行账户成功", list);
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "获取银行账户失败", null);
            }
            return lycResult;
        }

        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetALLBanks()
        {
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                var list = db.Banks.Select(c => new { ID = c.ID, cBankName = c.cBankName}).ToList();
                lycResult.Data = new JsonResultModel(true, "获取银行列表成功", list);
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "获取银行列表失败", null);
            }
            return lycResult;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }


    }

    public class DTOUserBank
    {
        public int userBankID { get; set; }
        public int bankID { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string money { get; set; }
        public string cardNo { get; set; }
    }
}
