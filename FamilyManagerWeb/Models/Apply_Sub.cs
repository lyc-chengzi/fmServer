//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Apply_Sub
    {
        public Apply_Sub()
        {
            this.Apply_Sub_JieKuan = new HashSet<Apply_Sub_JieKuan>();
        }
    
        public int ID { get; set; }
        public int ApplyMain_BillCode { get; set; }
        public short CashOrBank { get; set; }
        public int FlowTypeID { get; set; }
        public string FlowTypeName { get; set; }
        public string InOutType { get; set; }
        public Nullable<int> FeeItemID { get; set; }
        public string FeeItemName { get; set; }
        public decimal iMoney { get; set; }
        public Nullable<int> UserBankID { get; set; }
        public string BChange { get; set; }
        public string BJieKuan { get; set; }
        public string BHuanKuan { get; set; }
        public string CAdd { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CSouce { get; set; }
        public string CLocation { get; set; }
    
        public virtual Apply_Main Apply_Main { get; set; }
        public virtual ICollection<Apply_Sub_JieKuan> Apply_Sub_JieKuan { get; set; }
    }
}
