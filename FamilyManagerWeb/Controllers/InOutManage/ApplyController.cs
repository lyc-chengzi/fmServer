using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using Newtonsoft.Json;
using BaseFunction;
namespace FamilyManagerWeb.Controllers
{
    public class ApplyController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/InOutManage/Apply/";
        private const int currentPage = 1;
        private const int pageSize = 31;
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        #region 增删改查操作
        //用户收入列表
        public ActionResult List()
        {
            BindComboxUser();
            ViewBag.beginDate = "";
            ViewBag.endDate = "";
            List<Apply_Main> list = GetApply_MainList(1, null, null, null);
            return View(viewFolder + "List.cshtml", list);
        }
        //用户收入列表
        [HttpPost]
        public ActionResult List(Apply_Main am)
        {
            int pageNO = 1;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out pageNO);
            }
            BindComboxUser();
            string ApplyDate_Begin = Request.Form["ApplyDate_Begin"];
            string ApplyDate_End = Request.Form["ApplyDate_End"];
            ViewBag.beginDate = ApplyDate_Begin;
            ViewBag.endDate = ApplyDate_End;
            List<Apply_Main> list = GetApply_MainList(pageNO, am, ApplyDate_Begin, ApplyDate_End);
            return View(viewFolder + "List.cshtml", list);
        }
        //执行删除操作
        [HttpPost, ActionName("Delete")]
        public string DeleteConfirmed()
        {
            try
            {
                string ids = Request["ids"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(ids);
                foreach (int item in idList)
                {
                    Apply_Main am = db.Apply_Main.Find(item);
                    db.Apply_Main.Remove(am);
                }
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功", "UserSRList", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "删除失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }
        #endregion
        #region 记账页面
        //记账页面中转Action
        [ActionName("toCreate")]
        public Object Create(int businessTypeID, string inOutType)
        {
            if (businessTypeID == 0)
            {
                //跳转到现金记账页面
                ViewBag.FlowType = WebComm.GetFundFlowTypeList_Cash().Where(c => c.InOutType == "in" || c.InOutType == "out").ToList();
                return PartialView(viewFolder + "CashAccounting.cshtml");
            }
            else if (businessTypeID == 1)
            {
                //银行记账
                ViewBag.FlowType = WebComm.GetFundFlowTypeList_Bank().Where(c => c.InOutType == "in" || c.InOutType == "out").ToList();
                return PartialView(viewFolder + "BankAccounting.cshtml");
            }
            else
            {
                //跳转到转账页面
                ViewBag.FlowType = WebComm.GetFundFlowTypeList().Where(c => c.InOutType != "in" && c.InOutType != "out").ToList();
                return PartialView(viewFolder + "ZhuanZhang.cshtml");
            }
        }
        //执行现金记账
        [HttpPost]
        public string DoCashAccounting()
        {
            try
            {
                /***校验数据完整性***/
                if (Session[SessionList.FamilyManageUser.ToString()] == null)
                {
                    return WebComm.ReturnLogOutPage();
                }
                if (Request.Form["FlowTypeID"] == "" || Request.Form["FlowTypeID"] == "-1")
                {
                    throw new Exception("请选择资金流动类型！");
                }
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                //获取记账日期
                string applyDate = Request.Form["ApplyDate"];
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == Convert.ToInt32(Request.Form["FlowTypeID"])).Single();
                string flowTypeID = ffType.ID.ToString();
                //获取流动资金类型名称
                string flowTypeName = ffType.Name;
                //获取类型
                string InOutType = ffType.InOutType;
                //获取费用项目
                string feeItemID = Request.Form["search_Fee.FeeItemID"] == "" ? "0" : Request.Form["search_Fee.FeeItemID"];
                string feeItemName = Request.Form["search_Fee.FeeItemName"] == "" ? "" : Request.Form["search_Fee.FeeItemName"];
                //获取资金
                string iMoney = Request.Form["NowMoney"];
                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";
                //获取备注信息
                string cAdd = Request.Form["cAdd"];
                string sql = "exec proc_AddCashAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + loginUser.ID.ToString() + ",'" + isJieKuan + "','N','" + cAdd + "','PCWeb',''";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "记账成功！", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "记账失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }
        //执行银行记账
        [HttpPost]
        public string DoBankAccounting()
        {
            try
            {
                /***校验数据完整性***/
                if (Session[SessionList.FamilyManageUser.ToString()] == null)
                {
                    return WebComm.ReturnLogOutPage();
                }
                if (Request.Form["FlowTypeID"] == "" || Request.Form["FlowTypeID"] == "-1")
                {
                    throw new Exception("请选择资金流动类型！");
                }
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                //获取记账日期
                string applyDate = Request.Form["ApplyDate"];
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == Convert.ToInt32(Request.Form["FlowTypeID"])).Single();
                string flowTypeID = ffType.ID.ToString();
                //获取流动资金类型名称
                string flowTypeName = ffType.Name;
                //获取类型
                string InOutType = ffType.InOutType;
                //获取费用项目
                string feeItemID = Request.Form["search_Fee.FeeItemID"] == "" ? "0" : Request.Form["search_Fee.FeeItemID"];
                string feeItemName = Request.Form["search_Fee.FeeItemName"] == "" ? "" : Request.Form["search_Fee.FeeItemName"];
                //获取资金
                string iMoney = Request.Form["NowMoney"];
                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";
                //获取入账银行信息
                string inUserBankID = Request.Form["In_bank.UserBankID"] == "" ? "0" : Request.Form["In_bank.UserBankID"];
                //获取出账银行信息
                string outUserBankID = Request.Form["Out_bank.UserBankID"] == "" ? "0" : Request.Form["Out_bank.UserBankID"];
                //获取备注信息
                string cAdd = Request.Form["cAdd"];
                string sql = "exec proc_AddBankAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + loginUser.ID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + isJieKuan + "','N','" + cAdd + "','PCWeb',''";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "记账成功！", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "记账失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }
        //执行转账
        [HttpPost]
        public string DoZhuanZhang()
        {
            try
            {
                /***校验数据完整性***/
                if (Session[SessionList.FamilyManageUser.ToString()] == null)
                {
                    return WebComm.ReturnLogOutPage();
                }
                if (Request.Form["FlowTypeID"] == "" || Request.Form["FlowTypeID"] == "-1")
                {
                    throw new Exception("请选择资金流动类型！");
                }
                User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
                //获取记账日期
                string applyDate = Request.Form["ApplyDate"];
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == Convert.ToInt32(Request.Form["FlowTypeID"])).Single();
                string flowTypeID = ffType.ID.ToString();
                //获取流动资金类型名称
                string flowTypeName = ffType.Name;
                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = Request.Form["NowMoney"];
                //获取入账银行信息
                string inUserBankID = Request.Form["In_bank.UserBankID"] == "" ? "0" : Request.Form["In_bank.UserBankID"];
                //获取出账银行信息
                string outUserBankID = Request.Form["Out_bank.UserBankID"] == "" ? "0" : Request.Form["Out_bank.UserBankID"];
                //获取备注信息
                string cAdd = Request.Form["cAdd"];
                string sql = "exec proc_CashChange '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + iMoney + "," + loginUser.ID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + cAdd + "','PCWeb',''";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "记账成功！", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "记账失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }
        #endregion
        #region Ajax后台处理方法
        //根据用户选择的业务类型，显示资金流动类型
        [HttpPost]
        public string GetFlowTypeList()
        {
            int BusinessType = Convert.ToInt32(Request.Form["flowType"]);
            string result = "";
            List<FundFlowType> list = new List<FundFlowType>();
            if (BusinessType == 0)
            {
                list = WebComm.GetFundFlowTypeList_Cash();
            }
            else if (BusinessType == 1)
            {
                list = WebComm.GetFundFlowTypeList_Bank();
            }
            result = JsonConvert.SerializeObject(list);
            return result;
        }
        //获取资金流动类型的收入类型
        [HttpPost]
        public string GetFlowTypeInOut()
        {
            int flowID = Convert.ToInt32(Request.Form["FlowID"] ?? "0");
            string result = "";
            result = WebComm.GetFundFlowTypeList().Where(f => f.ID == flowID).Select(f => f.InOutType).SingleOrDefault();
            return result;
        }
        #endregion
        #region 私有方法
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /// <summary>
        /// 获取用户开户行列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="uBank">查询实体</param>
        /// <returns></returns>
        private List<Apply_Main> GetApply_MainList(int currentPage, Apply_Main AM, string beginDate, string endDate)
        {
            var amList = db.Apply_Main.AsQueryable();
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;
            //区分用户加载数据，如果不是超级管理员，只能查看自己的数据
            if (!UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                amList = amList.Where(ub => ub.ApplyUserID == loginUser.ID);
            }
            #region 条件筛选
            if (AM != null && !string.IsNullOrEmpty(AM.ApplyUserID.ToString()) && AM.ApplyUserID > 0)
            {
                amList = amList.Where(ub => ub.ApplyUserID == AM.ApplyUserID);
            }
            //如果用户选择了开始时间和结束时间
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                int[] beginArr = WebComm.GetIntArrayByStringArray(beginDate.Split('-'));
                int[] endArr = WebComm.GetIntArrayByStringArray(endDate.Split('-'));
                DateTime bDate, dDate;
                bDate = Convert.ToDateTime(beginDate);
                dDate = Convert.ToDateTime(endDate);
                int beginYear = beginArr[0];
                int beginMonth = beginArr[1];
                int beginDay = beginArr[2];
                int endYear = endArr[0];
                int endMonth = endArr[1];
                int endDay = endArr[2];
                //amList = amList.Where(a => a.iyear >= beginYear && a.imonth >= beginMonth && a.iday >= beginDay)
                // .Where(a => a.iyear <= endYear && a.imonth <= endMonth && a.iday <= endDay);
                amList = amList.Where(a => a.ApplyDate >= bDate && a.ApplyDate <= dDate);
            }
            //如果只选择了开始时间
            else if (!string.IsNullOrEmpty(beginDate) && string.IsNullOrEmpty(endDate))
            {
                DateTime bDate;
                bDate = Convert.ToDateTime(beginDate);
                //int[] beginArr = WebComm.GetIntArrayByStringArray(beginDate.Split('-'));
                //int beginYear = beginArr[0];
                //int beginMonth = beginArr[1];
                //int beginDay = beginArr[2];
                //amList = amList.Where(a => a.iyear >= beginYear && a.imonth >= beginMonth && a.iday >= beginDay);
                amList = amList.Where(a => a.ApplyDate >= bDate);
            }
            #endregion
            SetPagerOptions(amList.Count(), currentPage);
            List<Apply_Main> list = amList.OrderBy(b => b.ApplyDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            //添加合计行
            Apply_Main applySum = new Apply_Main();
            applySum.Apply_Sub = null;
            applySum.ApplyDate = new DateTime();
            applySum.ApplyInMoney = list.Sum(l => l.ApplyInMoney);
            applySum.ApplyOutMoney = list.Sum(l => l.ApplyOutMoney);
            applySum.ApplyUserID = loginUser.ID;
            applySum.CreateDate = null;
            applySum.ID = 0;
            applySum.iday = 0;
            applySum.imonth = 0;
            applySum.iNowCashMoney = 0;
            applySum.iyear = 0;
            applySum.User = null;
            list.Insert(0, applySum);
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