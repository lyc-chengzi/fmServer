using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FamilyManagerWeb.Models.ViewModels
{
    public class ConfigXml
    {
        public ConfigXml()
        {

            this.ConfigXmlPath = HttpRuntime.AppDomainAppPath + @"\Models\XmlModel\Config.xml";
            XDocument xdoc = XDocument.Load(this.ConfigXmlPath);
            this.thisDocument = xdoc;
            XElement root = xdoc.Root;
            this.BaseDataUpdateTime = root.Element("BaseDataUpdateTime").Value;
        }

        /// <summary>
        /// 当前加载的xml文档
        /// </summary>
        private XDocument thisDocument;

        /// <summary>
        /// 配置文件的存放路径
        /// </summary>
        public string ConfigXmlPath { get; private set; }

        /// <summary>
        /// 基础数据更新的时间点
        /// </summary>
        public string BaseDataUpdateTime { get; private set; }

        /// <summary>
        /// 更新基础数据更新时间点未当前时间
        /// </summary>
        /// <returns></returns>
        public bool SetNode_BaseDataUpdateTime()
        {
            bool result = true;

            try
            {
                this.thisDocument.Root.Element("BaseDataUpdateTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}