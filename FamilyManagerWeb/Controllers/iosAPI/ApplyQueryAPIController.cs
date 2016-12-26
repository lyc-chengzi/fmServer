using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using BaseFunction;
using System.Data;
using System.Data.SqlClient;

namespace FamilyManagerWeb.Controllers
{
    public class ApplyQueryAPIController : LycMVCController
    {
        public FamilyCaiWuDBEntities FDB { get; private set; }
        //
        // GET: /ApplyQueryAPI/

        public ContentResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" > GetApplyMainList(int userID, DateTime? startTime, DateTime? endTime, int flowTypeID, int feeitemID) <br/>");
            sb.AppendLine(" > GetApplySubList(int userID, int applyMainID = 0) <br/>");
            sb.AppendLine(" > GetTotalCaiFuInfo(int userID) --查询资产 </br>");
            return Content(sb.ToString());
        }

        /// <summary>
        /// 查询记账信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="startTime">记账开始时间</param>
        /// <param name="endTime">记账结束时间</param>
        /// <param name="flowTypeID">资金类型ID</param>
        /// <param name="feeitemID">费用科目类型ID</param>
        /// <returns></returns>
        public JsonResult GetApplyMainList(int userID, DateTime? startTime, DateTime? endTime, int flowTypeID = 0, int feeitemID = 0)
        {
            Thread.Sleep(500);
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                using (FDB = new FamilyCaiWuDBEntities())
                {
                    IQueryable<Apply_Main> qam = FDB.Apply_Main.Where(c => c.ApplyUserID == userID);
                    //筛选日期
                    if (startTime != null && endTime == null)
                    {
                        qam = qam.Where(c => c.ApplyDate >= startTime.Value);
                    }
                    else if (startTime == null && endTime != null)
                    {
                        qam = qam.Where(c => c.ApplyDate <= endTime.Value);
                    }
                    else if (startTime != null && endTime != null)
                    {
                        qam = qam.Where(c => c.ApplyDate >= startTime.Value && c.ApplyDate <= endTime.Value);
                    }

                    var result = qam.Select(c => new 
                    {
                        ApplyMainID = c.ID,
                        ApplyUserID = c.ApplyUserID,
                        ApplyDate = "",
                        ApplyDate_date = c.ApplyDate,
                        ApplyInMoney = c.ApplyInMoney,
                        ApplyOutMoney = c.ApplyOutMoney,
                        iyear = c.iyear,
                        imonth = c.imonth,
                        iday = c.iday,
                        iNowCashMoney = c.iNowCashMoney.Value
                    }).ToList();

                    List<QueryApplyMainModel> list = new List<QueryApplyMainModel>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        QueryApplyMainModel am = new QueryApplyMainModel();
                        var c = result[i];
                        am.ApplyMainID = c.ApplyMainID;
                        am.ApplyUserID = c.ApplyUserID;
                        am.ApplyDate = c.ApplyDate_date.ToString("yyyy-MM-dd");
                        am.ApplyInMoney = c.ApplyInMoney.ToString();
                        am.ApplyOutMoney = c.ApplyOutMoney.ToString();
                        am.iyear = c.iyear;
                        am.imonth = c.imonth;
                        am.iday = c.iday;
                        am.iNowCashMoney = c.iNowCashMoney.ToString();
                        list.Add(am);
                    }
                    var totalObj = new {totalIn = result.Sum(c=>c.ApplyInMoney).ToString(), totalOut = result.Sum(c=>c.ApplyOutMoney).ToString()};
                    var jsonObj = new { totalObj = totalObj , info = list};
                    lycResult.Data = new JsonResultModel(true, "账单查询成功", jsonObj);
                }
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "账单查询失败", null);
            }
            return lycResult;
        }

        public JsonResult GetApplySubList(int userID, int applyMainID = 0)
        {
            Thread.Sleep(500);
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                using (FDB = new FamilyCaiWuDBEntities())
                {
                    //查询出用户的银行信息
                    List<UserBank> ubList = FDB.UserBanks.Where(c => c.UserID == userID).ToList();

                    var result = (from s in FDB.Apply_Sub
                                    join sc in FDB.Apply_Sub_CashChange on s.ID equals sc.Apply_Sub_ID into ssc
                                    from left1 in ssc.DefaultIfEmpty()
                                    where s.ApplyMain_BillCode == applyMainID
                                    select new
                                    {
                                        ApplySubID = s.ID,
                                        ApplyMainID = s.ApplyMain_BillCode,
                                        CashOrBank = s.CashOrBank,
                                        FlowTypeID = s.FlowTypeID,
                                        FlowTypeName = s.FlowTypeName,
                                        InoutType = s.InOutType,
                                        FeeItemID = s.FeeItemID == null ? 0 : s.FeeItemID.Value,
                                        FeeItemName = s.FeeItemName,
                                        iMoney = s.iMoney,
                                        UserBankID = s.UserBankID == null ? 0 : s.UserBankID.Value,
                                        BChange = s.BChange,
                                        InUserBankID = left1.InUserBankID == null ? 0 : left1.InUserBankID.Value,
                                        OutUserBankID = left1.OutUserBankID == null ? 0 : left1.OutUserBankID.Value,
                                        CAdd = s.CAdd,
                                        CreateDate_date = s.CreateDate.Value
                                    }).ToList();
                    List<QueryApplySubModel> list = new List<QueryApplySubModel>();
                    foreach (var s in result)
                    {
                        QueryApplySubModel asm = new QueryApplySubModel();
                        asm.CreateDate = s.CreateDate_date.ToString("yyyy-MM-dd");
                        asm.ApplySubID = s.ApplySubID;
                        asm.ApplyMainID = s.ApplyMainID;
                        asm.CashOrBank = s.CashOrBank;
                        asm.FlowTypeID = s.FlowTypeID;
                        asm.FlowTypeName = s.FlowTypeName;
                        asm.InoutType = s.InoutType;
                        asm.FeeItemID = s.FeeItemID;
                        asm.FeeItemName = s.FeeItemName;
                        asm.iMoney = s.iMoney.ToString();
                        asm.UserBankID = s.UserBankID;
                        asm.UserBankName = "";
                        asm.BChange = s.BChange;
                        asm.InUserBankID = s.InUserBankID;
                        asm.InUserBankName = "";
                        asm.OutUserBankID = s.OutUserBankID;
                        asm.OutUserBankName = "";
                        asm.CAdd = s.CAdd;
                        foreach (var bank in ubList)
                        {
                            if (asm.UserBankID == bank.ID)
                            {
                                asm.UserBankName = bank.BankName;
                            }
                            if (asm.InUserBankID == bank.ID)
                            {
                                asm.InUserBankName = bank.BankName;
                            }
                            if (asm.OutUserBankID == bank.ID)
                            {
                                asm.OutUserBankName = bank.BankName;
                            }
                        }
                        list.Add(asm);
                    }
                    lycResult.Data = new JsonResultModel(true, "账单明细成功！", list);
                }  
            }
            catch
            {
                lycResult.Data = new JsonResultModel(false, "账单查询失败！", new object { });
            }
            return lycResult;
        }

        public JsonResult GetTotalCaiFuInfo(int userID)
        {
            Thread.Sleep(1000);
            LycJsonResult result = new LycJsonResult();
            try
            {
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@userID",userID)
                };
                DataTable dt = this.ExecStoredProcedureToDeataTable("QueryUserFullMoney", sp);
                DataRow row = dt.Rows[0];
                var _jsonObj = new { cashMoney = row["inowcashmoney"].ToString(), bankMoney = row["bankMoney"].ToString(), totalMoney = row["totalMoney"].ToString() };

                result.Data = new JsonResultModel { bSuccess = true, message = "获取资产信息成功", jsonObj = _jsonObj };               
            }
            catch (Exception ex)
            {
                result.Data = new JsonResultModel { bSuccess = false, message = "系统异常," + ex.Message, jsonObj = new object { } };
            }
            return result;
        }

        private DataTable ExecStoredProcedureToDeataTable(string procedureName, params SqlParameter[] par)
        {
            DataTable dt = new DataTable();
            try
            {
                string connStr = System.Web.Configuration.WebConfigurationManager.AppSettings["SqlCONNECTIONSTRING4"];
                SqlConnection conn;
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;
                    foreach (var item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                    
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "default");
                    dt = ds.Tables["default"];
                    cmd.Dispose();
                }
            }
            catch
            {
                throw new Exception("获取数据时错误！");
            }
            return dt;
        }
    
    }



    public class QueryApplyMainModel
    {
        public int ApplyMainID { get; set; }
        public int ApplyUserID { get; set; }
        public string ApplyDate { get; set; }
        public string ApplyInMoney { get; set; }
        public string ApplyOutMoney { get; set; }
        public int iyear { get; set; }
        public int imonth { get; set; }
        public int iday { get; set; }
        public string iNowCashMoney { get; set; }
        /*
        public string CreateDate { get; set; }
        public DateTime CreateDate_date { get; set; }*/
    }


    public class QueryApplySubModel
    {
        public int ApplySubID { get; set; }
        public int ApplyMainID { get; set; }
        public int CashOrBank { get; set; }
        public int FlowTypeID { get; set; }
        public string FlowTypeName { get; set; }
        public string InoutType { get; set; }
        public int FeeItemID { get; set; }
        public string FeeItemName { get; set; }
        public string iMoney { get; set; }
        public int UserBankID { get; set; }
        public string UserBankName { get; set; }
        public string BChange { get; set; }
        public int InUserBankID { get; set; }
        public string InUserBankName { get; set; }
        public int OutUserBankID { get; set; }
        public string OutUserBankName { get; set; }

        public string CAdd { get; set; }
        public string CreateDate { get; set; }
    }
}
