using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Linq;
using System.Xml.Linq;

using FamilyManagerWeb.Controllers;
using FamilyManagerWeb.Models;
using Newtonsoft.Json;

namespace FamilyManage.WebService
{
    /// <summary>
    /// UserBankService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/userbank/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class UserBankService : System.Web.Services.WebService
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        [WebMethod]
        public string HelloWorld()
        {
            return "{\"message\":\"HelloWorld\"}";
        }

        [WebMethod]
        public string GetUserBankList(int userID, string bankName, string bankNo, string bankCardType, int pageSize, int currentPage)
        {
            string jsonResult = "";
            try
            {
                //根据查询条件筛选数据
                var userBankList = db.UserBanks.Where(c => c.UserID == userID).AsQueryable();
                if (!string.IsNullOrEmpty(bankName))
                {
                    userBankList = userBankList.Where(c => c.BankName.Contains(bankName));
                }
                if (!string.IsNullOrEmpty(bankNo))
                {
                    userBankList = userBankList.Where(c => c.BankNo.Contains(bankNo));
                }
                if (!string.IsNullOrEmpty(bankCardType))
                {
                    userBankList = userBankList.Where(c => c.BankCardType.Contains(bankCardType));
                }
                int records = userBankList.Count();
                //将分页查询后的数据组织为可json序列化的对象
                var jsonObj = new
                              {
                                  pagesize = pageSize,
                                  totalRecords = records,
                                  currentpage = currentPage,
                                  rows = (from list in userBankList.OrderBy(c => c.BankID).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                                          select new
                                          {
                                              bankid = list.BankID,
                                              bankname = list.BankName,
                                              bankno = list.BankNo,
                                              bankcardtype = list.BankCardType,
                                              nowMoney = list.NowMoney
                                          }
                                         ).ToArray()
                              };
                if (records > 0)
                {
                    jsonResult = WebComm.ReturnJsonForExterior(true, "获取数据成功！", JsonConvert.SerializeObject(jsonObj));
                }
                else
                {
                    jsonResult = WebComm.ReturnJsonForExterior(false, "获取数据失败！", null);
                }
            }
            catch (Exception ex)
            {
                jsonResult = WebComm.ReturnJsonForExterior(false, "获取数据错误！" + ex.Message, null);
            }
            
            
            return jsonResult;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
