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
    public class SysModelController : Controller
    {
        private const string viewFolder = "~/Views/MainManage/SysModel/";
        private const int currentPage = 1;
        private const int pageSize = 20;
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();


        public ActionResult Index()
        {
            return View(viewFolder + "Index.cshtml", GetSysModelList(currentPage, null));
        }

        //用户开户银行列表
        [HttpPost]
        public ActionResult Index(SysModel sm)
        {
            int pageNO = 1;
            if (Request.Form["pageNum"] != null)
            {
                int.TryParse(Request.Form["pageNum"], out pageNO);
            }
            List<SysModel> list = GetSysModelList(pageNO, sm);
            return View(viewFolder + "Index.cshtml", list);
        }


        public ActionResult Create()
        {
            return View(viewFolder + "Create.cshtml");
        }

        //
        // POST: /SysModel/Create

        [HttpPost]
        public string Create(SysModel sysmodel)
        {
            try
            {
                sysmodel.IsFlag = true;
                db.SysModels.Add(sysmodel);
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "添加成功", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "添加失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        [ActionName("toEdit")]
        public ActionResult Edit(int id = 0)
        {
            SysModel sysmodel = db.SysModels.Find(id);
            if (sysmodel == null)
            {
                return HttpNotFound();
            }
            return View(viewFolder + "Edit.cshtml", sysmodel);
        }

        [HttpPost, ActionName("doEdit")]
        public string Edit(SysModel sysmodel)
        {
            try
            {
                db.Entry(sysmodel).State = EntityState.Modified;
                db.SaveChanges();

                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "修改成功", "", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "修改失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }


        [HttpPost, ActionName("Delete")]
        public string DeleteConfirmed()
        {
            try
            {
                string ids = Request["ids"] ?? "";
                int[] idList = WebComm.GetIntArrayByString(ids);
                foreach (int id in idList)
                {
                    SysModel bank = db.SysModels.Find(id);
                    bank.IsFlag = false;
                    db.Entry(bank).State = EntityState.Modified;
                }
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功", "", "", CallBackType.none, "");
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
        /// 获取用户开户行列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="uBank">查询实体</param>
        /// <returns></returns>
        private List<SysModel> GetSysModelList(int currentPage, SysModel model)
        {
            var smList = db.SysModels.Where(sm=>sm.IsFlag==true).AsQueryable();
            if (model != null)
            {
                if (model.SysModelClassID > 0)
                {
                    smList = smList.Where(sm => sm.SysModelClassID == model.SysModelClassID);
                }
            }
            SetPagerOptions(smList.Count(), currentPage);
            List<SysModel> list = smList.OrderBy(b => b.ID).Skip((currentPage - 1) * pageSize).Take(pageSize).OrderBy(sm => sm.SysModelClassID).ToList();

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