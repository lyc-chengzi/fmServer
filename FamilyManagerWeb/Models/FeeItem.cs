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
    
    public partial class FeeItem
    {
        public int FeeItemID { get; set; }
        public string FeeItemName { get; set; }
        public int parentID { get; set; }
        public bool isLast { get; set; }
        public Nullable<bool> isBase { get; set; }
    }
}