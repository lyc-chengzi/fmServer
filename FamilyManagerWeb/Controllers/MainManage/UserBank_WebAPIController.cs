using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FamilyManagerWeb.Models;
using Newtonsoft.Json;

namespace FamilyManagerWeb.Controllers.MainManage
{
    public class UserBank_WebAPIController : ApiController
    {
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        // GET api/UserBank_WebAPI
        public string GetUserBanks()
        {
            //var userbanks = db.UserBanks.Include(u => u.Bank).Select(c => new {bankName = c.BankName,BankCardCode = c.BankNo,BankType = c.BankCardType,BankMoney = c.NowMoney });
            //return JsonConvert.SerializeObject(userbanks);
            return "我是第一个方法";
        }

        // GET api/UserBank_WebAPI/5
        public string GetUserBank(int id,int page)
        {
            //string result = "null";
            //var userbank = db.UserBanks.Where(c=>c.UserID==userID).Select(c => new {bankName = c.BankName,BankCardCode = c.BankNo,BankType = c.BankCardType,BankMoney = c.NowMoney });
            //if (userbank.Count()>0)
            //{
            //    result = JsonConvert.SerializeObject(userbank);
            //}

            //return result;
            return "我是第二个方法,id:"+id+", page:"+page;
        }

        // PUT api/UserBank_WebAPI/5
        public HttpResponseMessage PutUserBank(int id, UserBank userbank)
        {
            if (ModelState.IsValid && id == userbank.ID)
            {
                db.Entry(userbank).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/UserBank_WebAPI
        public HttpResponseMessage PostUserBank(UserBank userbank)
        {
            if (ModelState.IsValid)
            {
                db.UserBanks.Add(userbank);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, userbank);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = userbank.ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/UserBank_WebAPI/5
        public HttpResponseMessage DeleteUserBank(int id)
        {
            UserBank userbank = db.UserBanks.Find(id);
            if (userbank == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.UserBanks.Remove(userbank);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, userbank);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}