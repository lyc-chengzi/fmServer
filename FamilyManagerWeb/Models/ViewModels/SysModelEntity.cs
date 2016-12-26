using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyManagerWeb.Models.ViewModels
{
    public class SysModelEntity
    {
        
        public int ID { get; set; }
        public string SysModelName { get; set; }
        public string SysModelPhyURL { get; set; }
        public string SysModelRouteName { get; set; }
        public string SysModelController { get; set; }
        public string SysModelAction { get; set; }
        public string SysModelRel { get; set; }
        public string SysModelTarget { get; set; }
        public string SysModelUrlPram { get; set; }
        public int SysModelClassID { get; set; }
        public int SysModelLevel { get; set; }
        public bool IsLast { get; set; }
        public bool IsHaveAction { get; set; }
        public bool IsFlag { get; set; }
    }
}