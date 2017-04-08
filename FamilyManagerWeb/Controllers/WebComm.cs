using FamilyManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FamilyManagerWeb.Controllers
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public static class WebComm
    {
        #region 私有属性
        private static List<FeeItem> listFeeItem = new List<FeeItem>();

        private static List<Models.FeeItem> lfi = new List<Models.FeeItem>();

        private static FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities(); 
        #endregion

        #region 对外开放方法

        /// <summary>
        /// 费用项目Xml配置文件的路径
        /// </summary>
        public static string FeeItemXmlPath = HttpRuntime.AppDomainAppPath + @"\Models\XmlModel\XML_FeeItem.xml";

        /// <summary>
        /// 账户类型Xml配置文件的路径
        /// </summary>
        public static string AccountTypeXmlPath = HttpRuntime.AppDomainAppPath + @"\Models\XmlModel\XML_AccountType.xml";

        /// <summary>
        /// 获得要返回的json提示格式
        /// </summary>
        /// <param name="status">返回状态</param>
        /// <param name="message">提示信息</param>
        /// <param name="navTabID">要重新加载的tabID</param>
        /// <param name="rel">要替换的标签rel属性</param>
        /// <param name="callBackType">回调函数</param>
        /// <param name="forwardURL">要跳转的url</param>
        /// <returns></returns>
        public static string ReturnAlertMessage(ActionReturnStatus status, string message, string navTabID, string rel, CallBackType callBackType, string forwardURL)
        {
            StringBuilder messageStr = new StringBuilder();
            messageStr.Append("{");
            messageStr.Append("\"statusCode\":").Append("\"").Append(GetStatusCode(status)).Append("\",");
            messageStr.Append("\"message\":").Append("\"").Append(message).Append("\",");
            messageStr.Append("\"navTabId\":").Append("\"").Append(navTabID).Append("\",");
            messageStr.Append("\"rel\":").Append("\"").Append(rel).Append("\",");
            messageStr.Append("\"callbackType\":").Append("\"").Append(callBackType.ToString() == "none" ? "" : callBackType.ToString()).Append("\",");
            messageStr.Append("\"forwardUrl\":").Append("\"").Append(forwardURL).Append("\"");
            messageStr.Append("}");

            return messageStr.ToString();
        }

        /// <summary>
        /// 对外部系统返回的json提示格式
        /// </summary>
        /// <param name="bSuccess">是否成功</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static string ReturnJsonForExterior(bool bSuccess, string message, string jsonObj)
        {
            StringBuilder messageStr = new StringBuilder();
            messageStr.Append("{");
            messageStr.Append("\"bSuccess\":").Append("").Append(bSuccess.ToString().ToLower()).Append(",");
            messageStr.Append("\"message\":").Append("\"").Append(message).Append("\",");
            messageStr.Append("\"jsonObj\":").Append(string.IsNullOrEmpty(jsonObj) ? "{}" : jsonObj).Append("");
            messageStr.Append("}");

            return messageStr.ToString();
        }

        /// <summary>
        /// 返回登录超时页面
        /// </summary>
        /// <returns></returns>
        public static string ReturnLogOutPage()
        {
            StringBuilder messageStr = new StringBuilder();
            messageStr.Append("{");
            messageStr.Append("\"statusCode\":").Append("\"").Append("301").Append("\",");
            messageStr.Append("\"message\":").Append("\"").Append("\u4f1a\u8bdd\u8d85\u65f6\uff0c\u8bf7\u91cd\u65b0\u767b\u5f55\u3002").Append("\",");
            messageStr.Append("\"navTabId\":").Append("\"").Append("\",");
            messageStr.Append("\"rel\":").Append("\"").Append("\",");
            messageStr.Append("\"callbackType\":").Append("\"").Append("\",");
            messageStr.Append("\"forwardUrl\":").Append("\"").Append("\"");
            messageStr.Append("}");

            return messageStr.ToString();
        }

        /// <summary>
        /// 跳到404错误处理页
        /// </summary>
        /// <returns></returns>
        public static string ReturnError404()
        {
            StringBuilder messageStr = new StringBuilder();
            messageStr.Append("{");
            messageStr.Append("\"statusCode\":").Append("\"").Append(GetStatusCode(ActionReturnStatus.失败)).Append("\",");
            messageStr.Append("\"message\":").Append("\"").Append("").Append("\",");
            messageStr.Append("\"navTabId\":").Append("\"").Append("").Append("\",");
            messageStr.Append("\"rel\":").Append("\"").Append("").Append("\",");
            messageStr.Append("\"callbackType\":").Append("\"").Append(CallBackType.closeCurrent.ToString() == "none" ? "" : CallBackType.closeCurrent.ToString()).Append("\",");
            messageStr.Append("\"forwardUrl\":").Append("\"").Append("/FamilyManage/ErrorHtml/Error404.html").Append("\"");
            messageStr.Append("}");

            return messageStr.ToString();
        }

        /// <summary>
        /// 把以,分割的字符串，转换成int类型数组
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public static int[] GetIntArrayByString(string strArray)
        {
            string[] s = strArray.Split(',');
            List<int> intList = new List<int>();
            int tempInt = 0;
            foreach (var item in s)
            {
                intList.Add(int.TryParse(item, out tempInt) == true ? tempInt : 0);
            }
            return intList.ToArray();
        }



        /// <summary>
        /// 将string类型数组转换成int类型数组
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public static int[] GetIntArrayByStringArray(string[] strArray)
        {
            int tempInt = 0;
            int[] intArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                int.TryParse(strArray[i], out tempInt);
                intArray[i] = tempInt;
            }
            return intArray;
        }

        /// <summary>
        /// MVC4绑定DWZ下拉框
        /// </summary>
        /// <param name="dataSource">数据源(value，text)</param>
        /// <param name="attrDic">返回控件html属性</param>
        /// <param name="listItem">返回控件项列表</param>
        public static void BindComBox(Dictionary<string, string> dataSource, out Dictionary<string, object> attrDic, out List<SelectListItem> listItem)
        {
            //设置选择用户下拉框html属性
            attrDic = new Dictionary<string, object>();
            attrDic.Add("class", "combox");

            //绑定值
            listItem = new List<SelectListItem>();
            SelectListItem liDefault = new SelectListItem();
            liDefault.Text = "全部";
            liDefault.Value = "-1";
            liDefault.Selected = true;
            listItem.Add(liDefault);

            foreach (var item in dataSource)
            {
                SelectListItem li = new SelectListItem();
                li.Text = item.Value;
                li.Value = item.Key;
                listItem.Add(li);
            }
        }

        /// <summary>
        /// 获取费用项目树
        /// </summary>
        /// <returns></returns>
        public static string GetFeeitemTree()
        {
            listFeeItem = GetFeeItemListByXml();
            return GetFeeItemTreeList(listFeeItem.Where(c=>c.FeeItemClassID==0).ToList());
        }

        private static string GetFeeItemTreeList(List<FeeItem> dataSource)
        {
            StringBuilder tree = new StringBuilder("");
            tree.Clear();
            foreach (FeeItem item in dataSource)
            {
                tree.Append("<li>");
                if (item.IsLast == false)
                {
                    tree.Append("<a href=\"javascript:\">").Append(item.FeeItemName).Append("</a>");
                    tree.Append("<ul>");
                    tree.Append(GetFeeItemTreeList(listFeeItem.Where(l => l.FeeItemClassID == item.FeeItemID).ToList()));
                    tree.Append("</ul>");
                }
                else
                {
                    tree.Append("<a href=\"javascript:\" onclick=\"$.bringBack({FeeItemID:'").Append(item.FeeItemID).Append("', FeeItemName:'").Append(item.FeeItemName).Append("'})\">").Append(item.FeeItemName).Append("</a>");
                }
                tree.Append("</li>");
            }

            return tree.ToString();
        }


        /// <summary>
        /// 获取费用项目树withsql
        /// </summary>
        /// <returns></returns>
        public static string GetFeeitemTreeWithSql()
        {
            lfi = db.FeeItem.ToList();
            return GetFeeItemTreeListWithSql(lfi.Where(c => c.parentID == 0).ToList());
        }

        private static string GetFeeItemTreeListWithSql(List<Models.FeeItem> dataSource)
        {
            StringBuilder tree = new StringBuilder("");
            tree.Clear();
            foreach (Models.FeeItem item in dataSource)
            {
                tree.Append("<li>");
                if (item.isLast == false)
                {
                    tree.Append("<a href=\"javascript:\">").Append(item.FeeItemName).Append("</a>");
                    tree.Append("<ul>");
                    tree.Append(GetFeeItemTreeListWithSql(lfi.Where(l => l.parentID == item.FeeItemID).ToList()));
                    tree.Append("</ul>");
                }
                else
                {
                    tree.Append("<a href=\"javascript:\" onclick=\"$.bringBack({FeeItemID:'").Append(item.FeeItemID).Append("', FeeItemName:'").Append(item.FeeItemName).Append("'})\">").Append(item.FeeItemName).Append("</a>");
                }
                tree.Append("</li>");
            }

            return tree.ToString();
        }
        

        /*
        private static string GetMenuList(List<SysModel> DataSource)
        {
            StringBuilder tree = new StringBuilder("");
            tree.Clear();
            foreach (SysModel item in DataSource)
            {
                tree.Append("<li>");
                if (item.IsLast == false)
                {
                    tree.Append("<a href=\"javascript:\">").Append(item.FeeItemName).Append("</a>");
                    tree.Append("<ul>");
                    tree.Append(GetFeeitemTree(GetFeeItemList().Where(l => l.FeeItemClassID == item.FeeItemID).ToList()));
                    tree.Append("</ul>");
                }
                else
                {
                    tree.Append("<a href=\"javascript:\" onclick=\"$.bringBack({FeeItemID:'").Append(item.FeeItemID).Append("', FeeItemName:'").Append(item.FeeItemName).Append("'})\">").Append(item.FeeItemName).Append("</a>");
                }
                tree.Append("</li>");
            }

            return tree.ToString();
        }
        */
        #endregion

        #region 基础数据配置

        #region 银行卡类型
        /// <summary>
        /// 获取银行卡类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetBankCardType()
        {
            List<string> list = new List<string>();
            list.Add("借记卡");
            list.Add("信用卡");
            list.Add("存折");
            return list;
        }
        #endregion

        #region 资金流动类型

        /// <summary>
        /// 获取资金流动类型
        /// </summary>
        /// <returns></returns>
        public static List<FundFlowType> GetFundFlowTypeList()
        {
            List<FundFlowType> list = new List<FundFlowType>();

            list.Add(new FundFlowType(1, "现金支出", 0, "out"));
            list.Add(new FundFlowType(2, "现金收入", 0, "in"));
            list.Add(new FundFlowType(3, "现金借出", 0, "out"));
            list.Add(new FundFlowType(4, "现金借入", 0, "in"));
            list.Add(new FundFlowType(5, "现金工资收入", 0, "in"));
            list.Add(new FundFlowType(6, "现金还款收入", 0, "in"));
            list.Add(new FundFlowType(7, "现金存入银行", 0, "存钱"));
            list.Add(new FundFlowType(8, "银行卡支出", 1, "out"));
            list.Add(new FundFlowType(9, "银行卡收入", 1, "in"));
            list.Add(new FundFlowType(10, "银行卡借出", 1, "out"));
            list.Add(new FundFlowType(11, "银行卡借入", 1, "in"));
            list.Add(new FundFlowType(12, "银行卡工资收入", 1, "in"));
            list.Add(new FundFlowType(13, "银行还款收入", 1, "in"));
            list.Add(new FundFlowType(14, "银行卡内部转账", 1, "内部转账"));
            list.Add(new FundFlowType(15, "银行卡取现", 1, "取现"));
            list.Add(new FundFlowType(16, "信用卡现金透支", 1, "取现"));
            list.Add(new FundFlowType(17, "信用卡刷卡透支", 1, "out", 100));
            list.Add(new FundFlowType(18, "信用卡还贷", 1, "存钱"));
            list.Add(new FundFlowType(19, "现金还款支出", 0, "out"));
            list.Add(new FundFlowType(20, "银行卡还款支出", 1, "out"));
            list.Add(new FundFlowType(21, "公积金收入", 1, "in"));

            return list.OrderByDescending(c => c.OrderNumber).ToList();
        }

        /// <summary>
        /// 获取现金资金流动类型
        /// </summary>
        /// <returns></returns>
        public static List<FundFlowType> GetFundFlowTypeList_Cash()
        {
            List<FundFlowType> list = GetFundFlowTypeList().Where(l => l.FlowType == 0).ToList();

            return list;
        }

        /// <summary>
        /// 获取银行资金流动类型
        /// </summary>
        /// <returns></returns>
        public static List<FundFlowType> GetFundFlowTypeList_Bank()
        {
            List<FundFlowType> list = GetFundFlowTypeList().Where(l => l.FlowType == 1).OrderByDescending(l => l.OrderNumber).ToList();
            return list;
        }
        #endregion

        #region 费用项目
        /// <summary>
        /// 根据配置的xml文件,获取所有费用项目
        /// </summary>
        /// <returns></returns>
        public static List<FeeItem> GetFeeItemListByXml()
        {
            List<FeeItem> list = new List<FeeItem>();

            XDocument xdoc = XDocument.Load(FeeItemXmlPath);
            XElement root = xdoc.Root;
            list = root.Elements("FeeItem").Select(c =>
                new FeeItem(int.Parse(c.Element("FeeItemID").Value),
                            c.Element("FeeItemName").Value,
                            int.Parse(c.Element("FeeItemClassID").Value),
                            bool.Parse(c.Element("IsLast").Value)
                           )
            ).ToList<FeeItem>();
           

            return list;
        }

        /// <summary>
        /// 根据数据库,获取所有费用项目
        /// </summary>
        /// <returns></returns>
        public static List<FeeItem> GetFeeItemListByDB()
        {
            List<FeeItem> list = new List<FeeItem>();

            List<Models.FeeItem> lmf = new List<Models.FeeItem>();
            lmf = db.FeeItem.ToList();
            foreach (var item in lmf)
            {
                var fi = new FeeItem(item.FeeItemID, item.FeeItemName, item.parentID, item.isLast);
                list.Add(fi);
            }

            return list;
        }

        /// <summary>
        /// 根据配置的xml文件,获取所有账户类型
        /// </summary>
        /// <returns></returns>
        public static List<AccountType> GetAccountListByXml()
        {
            List<AccountType> list = new List<AccountType>();

            XDocument xdoc = XDocument.Load(AccountTypeXmlPath);
            XElement root = xdoc.Root;
            list = root.Elements("AccountType").Select(c =>
                new AccountType(int.Parse(c.Element("TypeID").Value),
                            c.Element("TypeName").Value)
            ).ToList<AccountType>();
            return list;
        }


        /// <summary>
        /// 获取所有费用项目
        /// </summary>
        /// <returns></returns>
        public static List<FeeItem> GetFeeItemList()
        {
            List<FeeItem> list = new List<FeeItem>();

            list.Add(new FeeItem(1, "基本生活开销", 0, false));
            list.Add(new FeeItem(2, "服装开销", 0, false));
            list.Add(new FeeItem(3, "娱乐开销", 0, false));
            list.Add(new FeeItem(4, "网上购物", 0, false));
            list.Add(new FeeItem(5, "数码产品", 0, false));
            list.Add(new FeeItem(6, "家居产品", 0, false));

            /******************基本生活开销******************/
            list.Add(new FeeItem(101, "早饭", 1, true));
            list.Add(new FeeItem(102, "午饭", 1, true));
            list.Add(new FeeItem(103, "晚饭", 1, true));
            list.Add(new FeeItem(104, "零食", 1, true));
            list.Add(new FeeItem(105, "超市", 1, true));
            list.Add(new FeeItem(106, "借给别人钱", 1, true));
            list.Add(new FeeItem(107, "还给别人钱", 1, true));
            list.Add(new FeeItem(108, "银行手续费", 1, true));
            list.Add(new FeeItem(109, "杂物开销", 1, true));

            /******************服装开销******************/
            list.Add(new FeeItem(201, "帽子", 2, true));
            list.Add(new FeeItem(202, "眼镜", 2, true));
            list.Add(new FeeItem(203, "围巾", 2, true));
            list.Add(new FeeItem(204, "上衣", 2, true));
            list.Add(new FeeItem(205, "裤子", 2, true));
            list.Add(new FeeItem(206, "内衣", 2, true));
            list.Add(new FeeItem(207, "鞋子", 2, true));
            list.Add(new FeeItem(208, "袜子", 2, true));

            /******************娱乐开销******************/
            list.Add(new FeeItem(301, "看电影", 3, true));
            list.Add(new FeeItem(302, "电玩", 3, true));
            list.Add(new FeeItem(303, "彩票", 3, true));
            list.Add(new FeeItem(304, "游乐园", 3, true));

            /******************网上购物******************/
            list.Add(new FeeItem(401, "淘宝购物", 4, true));

            /******************数码产品******************/
            list.Add(new FeeItem(501, "手机", 5, true));
            list.Add(new FeeItem(502, "电脑", 5, true));
            list.Add(new FeeItem(503, "相机", 5, true));

            /******************家居产品******************/
            list.Add(new FeeItem(601, "冰箱", 6, true));
            list.Add(new FeeItem(602, "电视机", 6, true));
            list.Add(new FeeItem(603, "橱柜", 6, true));
            list.Add(new FeeItem(604, "茶几", 6, true));

            return list;
        }
        #endregion

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取状态代码
        /// </summary>
        /// <param name="status">状态枚举</param>
        /// <returns></returns>
        private static string GetStatusCode(ActionReturnStatus status)
        {
            string result = "";
            switch (status)
            {
                case ActionReturnStatus.成功:
                    result = "200";
                    break;
                case ActionReturnStatus.失败:
                    result = "300";
                    break;
                case ActionReturnStatus.操作超时:
                    result = "301";
                    break;
                default:
                    result = "300";
                    break;
            }
            return result;
        }
        #endregion

    }


    #region 公共枚举

    /// <summary>
    /// 账号类型实体类
    /// </summary>
    public class AccountType 
    {
        public AccountType(int typeID, string typeName)
        {
            this.TypeID = typeID;
            this.TypeName = typeName;
        }

        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }

    public class FeeItem
    {
        public FeeItem(int id, string name, int classid, bool last)
        {
            this.FeeItemID = id;
            this.FeeItemName = name;
            this.FeeItemClassID = classid;
            this.IsLast = last;
        }
        /// <summary>
        /// 主键，费用项目唯一标示
        /// </summary>
        public int FeeItemID { get; set; }

        /// <summary>
        /// 费用项目名称
        /// </summary>
        public string FeeItemName { get; set; }

        /// <summary>
        /// 上级费用项目ID
        /// </summary>
        public int FeeItemClassID { get; set; }

        /// <summary>
        /// 是否末级费用项目
        /// </summary>
        public bool IsLast { get; set; }
    }

    /// <summary>
    /// 资金流动类型
    /// </summary>
    public class FundFlowType
    {
        public FundFlowType(int id, string name, int flowType, string inOutType)
        {
            this.ID = id;
            this.Name = name;
            this.FlowType = flowType;
            this.InOutType = inOutType;
            this.OrderNumber = 0;
        }
        public FundFlowType(int id, string name, int flowType, string inOutType, int order):this(id,name,flowType,inOutType)
        {
            this.OrderNumber = order;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 业务类型 => 0：现金业务;1：银行业务;
        /// </summary>
        public int FlowType { get; set; }
        /// <summary>
        /// 收入或支出类型；收入：in，支出：out，其他记录汉字，代表不涉及资金变动
        /// </summary>
        public string InOutType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNumber { get; set; }
    }

    /// <summary>
    /// 返回请求类型
    /// </summary>
    public enum ActionReturnStatus
    {
        成功,
        失败,
        操作超时
    }

    public enum CallBackType
    {
        closeCurrent,
        forward,
        UserList_Refresh,
        none
    }

    /// <summary>
    /// session列表枚举
    /// </summary>
    public enum SessionList
    {
        FamilyManageUser
    }

    public enum ControllerList
    { 
        Default,
        MainManage,
        InOutManage,
        Dialog,
        Report
    }

    /// <summary>
    /// 用户权限实体类
    /// </summary>
    public class UserPower
    {
        /// <summary>
        /// 超级管理员账户列表
        /// </summary>
        public static int[] adminUserCode = { 10000 };
    }

    /// <summary>
    /// 按钮权限类
    /// </summary>
    public class BottonPower
    {
        /// <summary>
        /// 添加按钮
        /// </summary>
        public bool btn_Add { get; set; }

        /// <summary>
        /// 修改按钮
        /// </summary>
        public bool btn_Edit { get; set; }

        /// <summary>
        /// 删除按钮
        /// </summary>
        public bool btn_Delete { get; set; }

        /// <summary>
        /// 导出按钮
        /// </summary>
        public bool btn_Export { get; set; }

        /// <summary>
        /// 导入按钮
        /// </summary>
        public bool btn_Import { get; set; }

        /// <summary>
        /// 查询按钮
        /// </summary>
        public bool btn_Query { get; set; }

        /// <summary>
        /// 高级查询按钮
        /// </summary>
        public bool btn_MoreQuery { get; set; }

        /// <summary>
        /// 审核按钮
        /// </summary>
        public bool btn_Check { get; set; }

        /// <summary>
        /// 弃审按钮
        /// </summary>
        public bool btn_UnCheck { get; set; }
    }

    /// <summary>
    /// 用户菜单权限
    /// </summary>
    public class MenuPower
    {
        public bool menu_UserList { get; set; }
        public bool menu_UserAdd { get; set; }
    }

    public class JsonResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool bSuccess { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 额外的json信息
        /// </summary>
        public object jsonObj { get; set; }

        public JsonResultModel() { }

        public JsonResultModel(bool bSuccess, string message, object jsonObj)
        {
            this.bSuccess = bSuccess;
            this.message = message;
            this.jsonObj = jsonObj;
        }
    }

    public class LycJsonResult : JsonResult
    {
        public LycJsonResult()
            : base()
        {
            this.MaxJsonLength = 80000000;
            this.ContentEncoding = Encoding.UTF8;
            this.ContentType = "application/json";
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
        }
    }
    #endregion

}
