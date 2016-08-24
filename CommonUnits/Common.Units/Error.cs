using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
namespace Common.Units
{
    /// <summary>
    /// Author;Gavin
    /// Create Date: 2016-08-23
    /// Description:错误日志类
    /// </summary>
    public class Error
    {
        public static void WriteErrorXml(string strErrorInfo) 
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                DateTime now = System.DateTime.Now;
                //String fileName = Utility.Format.FormatDateTime(now);
                String path = @"C:\elaqlog\Error\";
                String fileUrl = Path.Combine(path, now.ToString("yyyyMMdd") + ".xml");// @"D:\Error\www.sipfam.com\" + fileName + ".xml";

                if (File.Exists(fileUrl))
                {
                    //根据模板创建文件
                    xmldoc.Load(fileUrl);
                }
                else
                {
                    //加入XML的声明段落
                    XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                    xmldoc.AppendChild(xmlnode);

                    //加入一个根元素					
                    XmlElement xmlelem = xmldoc.CreateElement("", "ROOT", "");
                    xmldoc.AppendChild(xmlelem);
                }

                //添加父节点
                XmlElement elemError = xmldoc.CreateElement("", "ERROR", "");
                xmldoc.ChildNodes.Item(1).AppendChild(elemError);

                //添加错误信息
                XmlElement elemMsg = xmldoc.CreateElement("", "MSG", "");
                XmlText textMsg = xmldoc.CreateTextNode(strErrorInfo);
                elemMsg.AppendChild(textMsg);
                elemError.AppendChild(elemMsg);

                //添加时间
                XmlElement elemTime = xmldoc.CreateElement("", "TIME", "");
                XmlText textTime = xmldoc.CreateTextNode(now.ToString());
                elemTime.AppendChild(textTime);
                elemError.AppendChild(elemTime);

                xmldoc.Save(fileUrl);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Utility.Error.WriteErrorXml:" + ex.Message);
            }
        }
    }
}
