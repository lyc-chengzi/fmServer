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
    
    public partial class UserBank
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string BankNo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string BankCardType { get; set; }
        public Nullable<decimal> NowMoney { get; set; }
    
        public virtual Bank Bank { get; set; }
    }
}
