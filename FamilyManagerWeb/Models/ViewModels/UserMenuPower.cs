using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyManagerWeb.Models.ViewModels
{
    public class UserMenuPowerEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }
}