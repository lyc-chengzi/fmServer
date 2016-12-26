using BaseFunction;
using FamilyManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using FamilyManagerWeb.Models.ViewModels;
using Newtonsoft.Json;
using System.Data.SqlClient;
namespace FamilyManagerWeb.Controllers
{
    public class ApplyAPIController : LycMVCController
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            Apply_temp_sync_VM ats = new Apply_temp_sync_VM();
            ats.userID = 13;
            ats.applyDate = new DateTime(2015, 7, 1);
            ats.keepType = "现金记账";
            ats.flowTypeID = 1;
            ats.flowTypeName = "现金支出";
            ats.inOutType = "out";
            ats.feeItemID = 101;
            ats.feeItemName = "早饭";
            ats.imoney = 10;
            ats.inUserBankID = 12;
            ats.outUserBankID = 22;
            ats.cAdd = "无备注";
            sb.AppendLine(" > DoCashAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string cAdd) <br/>");
            sb.AppendLine(" > DoBankAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string inUBID, string outUBID, string cAdd) <br/>");
            sb.AppendLine(" > DoZhuanZhang(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string inUBID, string outUBID, string cAdd) <br/>");
            sb.AppendLine(" > SyncApplyInfo(string jsonStr) <br/>");
            sb.AppendLine(" > Apply_temp_sync_VM:<br/>").AppendLine(JsonConvert.SerializeObject(ats)).Append(" <br/>");
            return Content(sb.ToString());
        }
        //现金记账API
        public string DoCashAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string cAdd, string location)
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
                //参数设置
                SqlParameter[] sp1 = new SqlParameter[]
                {
                new SqlParameter{ParameterName = "@applyDate",Value = applyDate},
                new SqlParameter{ParameterName = "@flowTypeID",Value = flowTypeID},
                new SqlParameter{ParameterName = "@flowTypeName",Value = flowTypeName},
                new SqlParameter{ParameterName = "@InOutType",Value = InOutType},
                new SqlParameter{ParameterName = "@FeeItemID",Value = feeItemID},
                new SqlParameter{ParameterName = "@FeeItemName",Value = feeItemName},
                new SqlParameter{ParameterName = "@iMoney",Value = iMoney},
                new SqlParameter{ParameterName = "@UserID",Value = userID},
                new SqlParameter{ParameterName = "@BJieKuan",Value = isJieKuan},
                new SqlParameter{ParameterName = "@BHuanKuan",Value = "N"},
                new SqlParameter{ParameterName = "@CAdd",Value = cAdd},
                new SqlParameter{ParameterName = "@CSouce",Value = APPLY_DATASOURCE_IOSAPP},
                new SqlParameter{ParameterName = "@CLocation",Value = location ?? ""}
                };
                //执行存储过程
                bool success = this.ExecStoredProcedure("proc_AddCashAccouting", sp1);
                if (success == true)
                {
                    result = WebComm.ReturnJsonForExterior(true, "现金记账成功！", "{}");
                }
                else
                {
                    result = WebComm.ReturnJsonForExterior(false, "现金记账失败！", "{}");
                }
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "现金记账失败！" + ex.Message, "{}");
            }
            return result;
        }
        //银行记账API
        public string DoBankAccounting(int userID, string ApplyDate, int FlowTypeID,
        string feeItemID, string feeItemName, string money,
        string inUBID, string outUBID, string cAdd, string location)
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
                //参数设置
                SqlParameter[] sp2 = new SqlParameter[]
                {
                new SqlParameter{ParameterName = "@applyDate",Value = applyDate},
                new SqlParameter{ParameterName = "@flowTypeID",Value = flowTypeID},
                new SqlParameter{ParameterName = "@flowTypeName",Value = flowTypeName},
                new SqlParameter{ParameterName = "@InOutType",Value = InOutType},
                new SqlParameter{ParameterName = "@FeeItemID",Value = feeItemID},
                new SqlParameter{ParameterName = "@FeeItemName",Value = feeItemName},
                new SqlParameter{ParameterName = "@iMoney",Value = iMoney},
                new SqlParameter{ParameterName = "@UserID",Value = userID},
                new SqlParameter{ParameterName = "@InUserBankID",Value = inUserBankID},
                new SqlParameter{ParameterName = "@OutUserBankID",Value = outUserBankID},
                new SqlParameter{ParameterName = "@BJieKuan",Value = isJieKuan},
                new SqlParameter{ParameterName = "@BHuanKuan",Value = "N"},
                new SqlParameter{ParameterName = "@CAdd",Value = cAdd},
                new SqlParameter{ParameterName = "@CSouce",Value = APPLY_DATASOURCE_IOSAPP},
                new SqlParameter{ParameterName = "@CLocation",Value = location ?? ""}
                };
                //执行存储过程
                bool success = this.ExecStoredProcedure("proc_AddBankAccouting", sp2);
                if (success)
                {
                    result = WebComm.ReturnJsonForExterior(true, "银行记账成功！", "{}");
                }
                else
                {
                    result = WebComm.ReturnJsonForExterior(true, "银行记账失败！", "{}");
                }
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "银行记账失败！" + ex.Message, "{}");
            }
            return result;
        }
        //内部转账API
        public string DoZhuanZhang(int userID, string ApplyDate, int FlowTypeID,
        string feeItemID, string feeItemName, string money,
        string inUBID, string outUBID, string cAdd, string location)
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
                //参数设置
                SqlParameter[] sp3 = new SqlParameter[]
                {
                new SqlParameter{ParameterName = "@applyDate",Value = applyDate},
                new SqlParameter{ParameterName = "@flowTypeID",Value = flowTypeID},
                new SqlParameter{ParameterName = "@flowTypeName",Value = flowTypeName},
                new SqlParameter{ParameterName = "@InOutType",Value = InOutType},
                new SqlParameter{ParameterName = "@iMoney",Value = iMoney},
                new SqlParameter{ParameterName = "@UserID",Value = userID},
                new SqlParameter{ParameterName = "@InUserBankID",Value = inUserBankID},
                new SqlParameter{ParameterName = "@OutUserBankID",Value = outUserBankID},
                new SqlParameter{ParameterName = "@CAdd",Value = cAdd},
                new SqlParameter{ParameterName = "@CSouce",Value = APPLY_DATASOURCE_IOSAPP},
                new SqlParameter{ParameterName = "@CLocation",Value = location ?? ""}
                };
                //执行存储过程
                bool success = this.ExecStoredProcedure("proc_CashChange", sp3);
                if (success)
                {
                    result = WebComm.ReturnJsonForExterior(true, "转账记账成功！", "{}");
                }
                else
                {
                    result = WebComm.ReturnJsonForExterior(true, "转账记账失败！", "{}");
                }
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "转账记账失败！" + ex.Message, "{}");
            }
            return result;
        }
        //同步记账信息API
        public JsonResult SyncApplyInfo(string jsonStr)
        {
            LycJsonResult lycResult = new LycJsonResult();
            //1、先将要同步的记账信息写入同步临时表
            try
            {
                List<Apply_temp_sync_VM> lvm = new List<Apply_temp_sync_VM>();
                lvm = JsonConvert.DeserializeObject<List<Apply_temp_sync_VM>>(jsonStr);
                //转换完成后执行存储过程
                bool success = this.SyncApplyWithProcAndTransaction(lvm);
                if (success)
                {
                    lycResult.Data = new JsonResultModel { bSuccess = true, message = "同步成功！", jsonObj = null };
                }
                else
                {
                    lycResult.Data = new JsonResultModel { bSuccess = false, message = "同步失败！", jsonObj = null };
                }
            }
            catch (Exception ex)
            {
                lycResult.Data = new JsonResultModel { bSuccess = false, message = "写入同步表失败：" + ex.Message, jsonObj = null };
            }
            return lycResult;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }
        /*
        DateTime nowDate = DateTime.Now;
        string applyGuid = Guid.NewGuid().ToString();
        List<apply_temp_sync> atsList = new List<apply_temp_sync>();
        foreach (var item in lvm)
        {
        apply_temp_sync ats = new apply_temp_sync();
        ats.applyDate = item.applyDate;
        ats.userID = item.userID;
        ats.keepType = item.keepType;
        ats.flowTypeID = item.flowTypeID;
        ats.flowTypeName = item.flowTypeName;
        ats.InOutType = item.inOutType;
        ats.FeeItemID = item.feeItemID;
        ats.FeeItemName = item.feeItemName;
        ats.imoney = item.imoney;
        ats.InUserBankID = item.inUserBankID;
        ats.OutUserBankID = item.outUserBankID;
        ats.CAdd = item.cAdd;
        ats.applyGUID = applyGuid;
        ats.applyGUIDDate = nowDate;
        db.apply_temp_sync.Add(ats);
        }
        db.SaveChanges();
        * */
    }
}