using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using FamilyManagerWeb.Models.ViewModels;
using FamilyManagerWeb.Models.ViewModels.highchartsModel;
using Newtonsoft.Json;
using System.Collections;

namespace FamilyManagerWeb.Controllers
{
    public class ReportController : Controller
    {
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        public ActionResult test() 
        {
            return View();
        }

        public ActionResult TestEasyUI()
        {
            return View();
        }

        public ActionResult YearTongQi()
        {
            
            //取出所有年份的数据
            var _data = from c in db.Apply_Main
                       join s in db.Apply_Sub on c.ID equals s.ApplyMain_BillCode
                       where s.InOutType == "out" && s.ID != 2804 && s.ID != 2805
                       group s by new { iyear = c.iyear, imonth = c.imonth } into g
                       orderby g.Key.iyear, g.Key.imonth
                       select new YearTongQiModel
                       {
                           iyear = g.Key.iyear,
                           imonth = g.Key.imonth,
                           outMoney = g.Sum(c => c.iMoney)
                       };
            var dataList = _data.ToList();
            //取出所有年份
            var years = dataList.Select(c => c.iyear).Distinct().ToList();
            //将结果排序，按年份写入数组
            ArrayList resultList = new ArrayList();
            //取出各个年份最小月份和最大月份
            foreach (var year in years)
            {
                //取得当前年份的所有数据
                var thisYearData = dataList.Where(c => c.iyear == year).ToList();
                //取得当前年份所有月份
                List<int> thisYearMonthList = thisYearData.Select(c => c.imonth).ToList();
                //取出最小月份
                int minMonth = thisYearMonthList.Min();
                //如果当前年份的最小月份大于1，把之前的月份补充上，支出为0
                if (minMonth > 1)
                {
                    for (int i = minMonth - 1; i >= 1; i--)
                    {
                        thisYearData.Insert(0, new YearTongQiModel { iyear = year, imonth = i, outMoney = 0 });
                    }
                }
                //取出最大月份
                int maxMonth = thisYearMonthList.Max();
                //如果当前年份的最大月份小于12，把之后的月份补全，支出为0
                if (maxMonth < 12)
                {
                    for (int i = maxMonth + 1; i <= 12; i++)
                    {
                        thisYearData.Add(new YearTongQiModel { iyear = year, imonth = i, outMoney = 0 });
                    }
                }
                //将当前年份的数据加到数组中
                resultList.Add(thisYearData);
            }
            //highcharts线性图使用的数据格式
            List<BaseLineSeriesModel> lbs = new List<BaseLineSeriesModel>();
            foreach (var item in resultList)
            {
                BaseLineSeriesModel bsm = new BaseLineSeriesModel();
                var data = (List<YearTongQiModel>)item;
                bsm.name = data[0].iyear + " 年";
                bsm.data = new ArrayList();
                for (int i = 0; i <= data.Count - 1; i++)
                {
                    bsm.data.Add(data[i].outMoney);
                }
                lbs.Add(bsm);
            }
            ViewBag.ResutlData = lbs;
            return View();
        }
        /// <summary>
        /// 查询某一个月份的消费明细
        /// </summary>
        /// <param name="iyear">年</param>
        /// <param name="imonth">月</param>
        /// <returns></returns>
        public JsonResult GetMonthApplyOutInfo(int iyear, int imonth)
        {
            System.Threading.Thread.Sleep(500);
            LycJsonResult lycResult = new LycJsonResult();
            try
            {
                var list = (from a in db.Apply_Main
                            join b in db.Apply_Sub on a.ID equals b.ApplyMain_BillCode
                            into left1
                            from r in left1.DefaultIfEmpty()
                            where a.iyear == iyear && a.imonth == imonth && r.FeeItemID > 0
                            select new
                            {
                                iyear = a.iyear,
                                imonth = a.imonth,
                                iday = a.iday,
                                feeItemName = r.FeeItemName,
                                imoney = r.iMoney
                            }).ToList();
                
                lycResult.Data = new JsonResultModel { bSuccess = true, message = "查询成功", jsonObj = list };
            }
            catch
            {
                lycResult.Data = new JsonResultModel { bSuccess = false, message = "查询失败", jsonObj = new ArrayList() };
            }
            return lycResult;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    public class YearTongQiModel
    {
        public int iyear { get; set; }
        public int imonth { get; set; }
        public decimal outMoney { get; set; }
    }
}