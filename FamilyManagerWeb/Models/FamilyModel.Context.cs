﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamilyManagerWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class FamilyCaiWuDBEntities : DbContext
    {
        public FamilyCaiWuDBEntities()
            : base("name=FamilyCaiWuDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Apply_Main> Apply_Main { get; set; }
        public DbSet<Apply_Sub> Apply_Sub { get; set; }
        public DbSet<Apply_Sub_CashChange> Apply_Sub_CashChange { get; set; }
        public DbSet<Apply_Sub_JieKuan> Apply_Sub_JieKuan { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SysModel> SysModels { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserBank> UserBanks { get; set; }
        public DbSet<UserModelPower> UserModelPowers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<apply_temp_sync> apply_temp_sync { get; set; }
        public DbSet<FeeItem> FeeItem { get; set; }
    
        public virtual ObjectResult<Nullable<int>> proc_AddBankAccouting(string applyDate, Nullable<int> flowTypeID, string flowTypeName, string inOutType, Nullable<int> feeItemID, string feeItemName, Nullable<decimal> iMoney, Nullable<int> userID, Nullable<int> inUserBankID, Nullable<int> outUserBankID, string bJieKuan, string bHuanKuan, string cAdd, string cSouce, string cLocation)
        {
            var applyDateParameter = applyDate != null ?
                new ObjectParameter("applyDate", applyDate) :
                new ObjectParameter("applyDate", typeof(string));
    
            var flowTypeIDParameter = flowTypeID.HasValue ?
                new ObjectParameter("flowTypeID", flowTypeID) :
                new ObjectParameter("flowTypeID", typeof(int));
    
            var flowTypeNameParameter = flowTypeName != null ?
                new ObjectParameter("flowTypeName", flowTypeName) :
                new ObjectParameter("flowTypeName", typeof(string));
    
            var inOutTypeParameter = inOutType != null ?
                new ObjectParameter("InOutType", inOutType) :
                new ObjectParameter("InOutType", typeof(string));
    
            var feeItemIDParameter = feeItemID.HasValue ?
                new ObjectParameter("FeeItemID", feeItemID) :
                new ObjectParameter("FeeItemID", typeof(int));
    
            var feeItemNameParameter = feeItemName != null ?
                new ObjectParameter("FeeItemName", feeItemName) :
                new ObjectParameter("FeeItemName", typeof(string));
    
            var iMoneyParameter = iMoney.HasValue ?
                new ObjectParameter("iMoney", iMoney) :
                new ObjectParameter("iMoney", typeof(decimal));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var inUserBankIDParameter = inUserBankID.HasValue ?
                new ObjectParameter("InUserBankID", inUserBankID) :
                new ObjectParameter("InUserBankID", typeof(int));
    
            var outUserBankIDParameter = outUserBankID.HasValue ?
                new ObjectParameter("OutUserBankID", outUserBankID) :
                new ObjectParameter("OutUserBankID", typeof(int));
    
            var bJieKuanParameter = bJieKuan != null ?
                new ObjectParameter("BJieKuan", bJieKuan) :
                new ObjectParameter("BJieKuan", typeof(string));
    
            var bHuanKuanParameter = bHuanKuan != null ?
                new ObjectParameter("BHuanKuan", bHuanKuan) :
                new ObjectParameter("BHuanKuan", typeof(string));
    
            var cAddParameter = cAdd != null ?
                new ObjectParameter("CAdd", cAdd) :
                new ObjectParameter("CAdd", typeof(string));
    
            var cSouceParameter = cSouce != null ?
                new ObjectParameter("CSouce", cSouce) :
                new ObjectParameter("CSouce", typeof(string));
    
            var cLocationParameter = cLocation != null ?
                new ObjectParameter("CLocation", cLocation) :
                new ObjectParameter("CLocation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("proc_AddBankAccouting", applyDateParameter, flowTypeIDParameter, flowTypeNameParameter, inOutTypeParameter, feeItemIDParameter, feeItemNameParameter, iMoneyParameter, userIDParameter, inUserBankIDParameter, outUserBankIDParameter, bJieKuanParameter, bHuanKuanParameter, cAddParameter, cSouceParameter, cLocationParameter);
        }
    
        public virtual int proc_AddCashAccouting(string applyDate, Nullable<int> flowTypeID, string flowTypeName, string inOutType, Nullable<int> feeItemID, string feeItemName, Nullable<decimal> iMoney, Nullable<int> userID, string bJieKuan, string bHuanKuan, string cAdd, string cSouce, string cLocation)
        {
            var applyDateParameter = applyDate != null ?
                new ObjectParameter("applyDate", applyDate) :
                new ObjectParameter("applyDate", typeof(string));
    
            var flowTypeIDParameter = flowTypeID.HasValue ?
                new ObjectParameter("flowTypeID", flowTypeID) :
                new ObjectParameter("flowTypeID", typeof(int));
    
            var flowTypeNameParameter = flowTypeName != null ?
                new ObjectParameter("flowTypeName", flowTypeName) :
                new ObjectParameter("flowTypeName", typeof(string));
    
            var inOutTypeParameter = inOutType != null ?
                new ObjectParameter("InOutType", inOutType) :
                new ObjectParameter("InOutType", typeof(string));
    
            var feeItemIDParameter = feeItemID.HasValue ?
                new ObjectParameter("FeeItemID", feeItemID) :
                new ObjectParameter("FeeItemID", typeof(int));
    
            var feeItemNameParameter = feeItemName != null ?
                new ObjectParameter("FeeItemName", feeItemName) :
                new ObjectParameter("FeeItemName", typeof(string));
    
            var iMoneyParameter = iMoney.HasValue ?
                new ObjectParameter("iMoney", iMoney) :
                new ObjectParameter("iMoney", typeof(decimal));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var bJieKuanParameter = bJieKuan != null ?
                new ObjectParameter("BJieKuan", bJieKuan) :
                new ObjectParameter("BJieKuan", typeof(string));
    
            var bHuanKuanParameter = bHuanKuan != null ?
                new ObjectParameter("BHuanKuan", bHuanKuan) :
                new ObjectParameter("BHuanKuan", typeof(string));
    
            var cAddParameter = cAdd != null ?
                new ObjectParameter("CAdd", cAdd) :
                new ObjectParameter("CAdd", typeof(string));
    
            var cSouceParameter = cSouce != null ?
                new ObjectParameter("CSouce", cSouce) :
                new ObjectParameter("CSouce", typeof(string));
    
            var cLocationParameter = cLocation != null ?
                new ObjectParameter("CLocation", cLocation) :
                new ObjectParameter("CLocation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_AddCashAccouting", applyDateParameter, flowTypeIDParameter, flowTypeNameParameter, inOutTypeParameter, feeItemIDParameter, feeItemNameParameter, iMoneyParameter, userIDParameter, bJieKuanParameter, bHuanKuanParameter, cAddParameter, cSouceParameter, cLocationParameter);
        }
    
        public virtual int proc_CashChange(string applyDate, Nullable<int> flowTypeID, string flowTypeName, string inOutType, Nullable<decimal> iMoney, Nullable<int> userID, Nullable<int> inUserBankID, Nullable<int> outUserBankID, string cAdd, string cSouce, string cLocation)
        {
            var applyDateParameter = applyDate != null ?
                new ObjectParameter("applyDate", applyDate) :
                new ObjectParameter("applyDate", typeof(string));
    
            var flowTypeIDParameter = flowTypeID.HasValue ?
                new ObjectParameter("flowTypeID", flowTypeID) :
                new ObjectParameter("flowTypeID", typeof(int));
    
            var flowTypeNameParameter = flowTypeName != null ?
                new ObjectParameter("flowTypeName", flowTypeName) :
                new ObjectParameter("flowTypeName", typeof(string));
    
            var inOutTypeParameter = inOutType != null ?
                new ObjectParameter("InOutType", inOutType) :
                new ObjectParameter("InOutType", typeof(string));
    
            var iMoneyParameter = iMoney.HasValue ?
                new ObjectParameter("iMoney", iMoney) :
                new ObjectParameter("iMoney", typeof(decimal));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var inUserBankIDParameter = inUserBankID.HasValue ?
                new ObjectParameter("InUserBankID", inUserBankID) :
                new ObjectParameter("InUserBankID", typeof(int));
    
            var outUserBankIDParameter = outUserBankID.HasValue ?
                new ObjectParameter("OutUserBankID", outUserBankID) :
                new ObjectParameter("OutUserBankID", typeof(int));
    
            var cAddParameter = cAdd != null ?
                new ObjectParameter("CAdd", cAdd) :
                new ObjectParameter("CAdd", typeof(string));
    
            var cSouceParameter = cSouce != null ?
                new ObjectParameter("CSouce", cSouce) :
                new ObjectParameter("CSouce", typeof(string));
    
            var cLocationParameter = cLocation != null ?
                new ObjectParameter("CLocation", cLocation) :
                new ObjectParameter("CLocation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_CashChange", applyDateParameter, flowTypeIDParameter, flowTypeNameParameter, inOutTypeParameter, iMoneyParameter, userIDParameter, inUserBankIDParameter, outUserBankIDParameter, cAddParameter, cSouceParameter, cLocationParameter);
        }
    
        public virtual int proc_DeleteAccouting(Nullable<int> subID)
        {
            var subIDParameter = subID.HasValue ?
                new ObjectParameter("SubID", subID) :
                new ObjectParameter("SubID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_DeleteAccouting", subIDParameter);
        }
    
        public virtual ObjectResult<QueryUserApplyInfo_Result> QueryUserApplyInfo(Nullable<int> year, Nullable<int> month, Nullable<int> userID)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<QueryUserApplyInfo_Result>("QueryUserApplyInfo", yearParameter, monthParameter, userIDParameter);
        }
    
        public virtual ObjectResult<QueryUserFullMoney_Result> QueryUserFullMoney(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<QueryUserFullMoney_Result>("QueryUserFullMoney", userIDParameter);
        }
    }
}
