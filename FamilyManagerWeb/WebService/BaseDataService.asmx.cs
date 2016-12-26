using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseFunction;
using FamilyManagerWeb.Controllers;
using Newtonsoft.Json;
using FamilyManagerWeb.Models;

namespace FamilyManage.WebService
{
    /// <summary>
    /// BaseDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/BaseData/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class BaseDataService : System.Web.Services.WebService
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        [WebMethod]
        public string GetFeeItem()
        {
            string result = "{}";
            try
            {
                result = WebComm.ReturnJsonForExterior(true, "获取费用科目成功！", JsonConvert.SerializeObject(WebComm.GetFeeItemListByXml().Where(c => c.IsLast == true)));
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "获取费用科目失败！" + ex.Message, null);
            }
            return result;
        }

        [WebMethod]
        public string GetFlowType(int typeID)
        {
            //typeID=0查询现金的资金类型，为1时查询银行相关的
            string result = "{}";
            string obj="";
            try
            {
                if (typeID == 0)
                {
                    obj = JsonConvert.SerializeObject(WebComm.GetFundFlowTypeList_Cash().Where(c => c.InOutType == "in" || c.InOutType == "out"));
                    
                }
                else if (typeID == 1)
                {
                    obj = JsonConvert.SerializeObject(WebComm.GetFundFlowTypeList_Bank().Where(c => c.InOutType == "in" || c.InOutType == "out"));
                }
                else
                {
                    obj = JsonConvert.SerializeObject(WebComm.GetFundFlowTypeList_Bank().Where(c => c.InOutType != "in" && c.InOutType != "out"));
                }
                result = WebComm.ReturnJsonForExterior(true, "获取资金类型成功！", obj);
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "获取资金类型失败！" + ex.Message, null);
            }

            return result;
        }

        [WebMethod]
        public string DoCashAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string cAdd)
        {

            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";

                //获取备注信息               

                string sql = "exec proc_AddCashAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + userID.ToString() + ",'" + isJieKuan + "','N','" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "现金记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "现金记账失败！" + ex.Message, "{}");
            }
            return result;
        }

        [WebMethod]
        public string DoBankAccounting(int userID, string ApplyDate, int FlowTypeID,
            string feeItemID, string feeItemName, string money, 
            string inUBID, string outUBID, string cAdd)
        {

            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";
                //获取入账银行信息
                string inUserBankID = inUBID;
                //获取出账银行信息
                string outUserBankID = outUBID;

                //获取备注信息               

                string sql = "exec proc_AddBankAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + userID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + isJieKuan + "','N','" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "银行记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "银行记账失败！" + ex.Message, "{}");
            }
            return result;
        }

        [WebMethod]
        public string DoZhuanZhang(int userID, string ApplyDate, int FlowTypeID, 
            string feeItemID, string feeItemName, string money,
            string inUBID,string outUBID, string cAdd)
        {

            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                //获取入账银行信息
                string inUserBankID = inUBID;
                //获取出账银行信息
                string outUserBankID = outUBID;

                //获取备注信息               

                string sql = "exec proc_CashChange '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + iMoney + "," + userID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "转账记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "转账记账失败！" + ex.Message, "{}");
            }
            return result;
        }

        [WebMethod]
        public string GetUserBankList(int userID)
        {
            string result = "{}";
            try
            {
                var list = from lu in db.UserBanks
                           where lu.UserID == userID
                           select new
                           {
                               userBankID = lu.ID,
                               bankID = lu.BankID,
                               bankName = lu.BankName,
                               bankType = lu.BankCardType,
                               money = lu.NowMoney,
                               cardNo = lu.BankNo
                           };
                result = WebComm.ReturnJsonForExterior(true, "获取银行账户成功！", JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "获取银行账户失败！" + ex.Message, "{}");
            }
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }
    }
}
