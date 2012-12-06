using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using KJFramework.Configurations.Objects;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     XML文件帮助器，提供了一系列读取自定义XML节点的基础操作。
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        ///     从一个文件中获取2级节点列表
        /// </summary>
        /// <param name="filename">XML配置文件</param>
        /// <param name="nodename">2级节点名称</param>
        /// <returns>返回2级节点列表</returns>
        public static XmlNodeList GetNode(String filename, String nodename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException("要解析的XML文件未找到!");
                }
                if (String.IsNullOrEmpty(nodename))
                {
                    throw new System.Exception("要解析的节点名称非法!");
                }
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode rootNode = document.DocumentElement;
                if (rootNode != null)
                {
                    return rootNode.SelectNodes(nodename);
                }
                return null;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("得到XML节点出现异常 ： " + e.Message);
                return null;
            }
        }

        /// <summary>
        ///     解析一个XML节点中的下一级的指定名称节点列表
        /// </summary>
        /// <param name="node">要解析的XML节点</param>
        /// <param name="nodename">要解析的次节点名称</param>
        /// <returns></returns>
        public static XmlNodeList GetNode(XmlNode node, String nodename)
        {
            if (node == null)
            {
                throw new System.Exception("要解析节点不能为空!");
            }
            if (String.IsNullOrEmpty(nodename))
            {
                throw new System.Exception("要解析的节点名称非法!");
            }
            return node.SelectNodes(nodename);
        }

        /// <summary>
        ///     解析一个XML节点中的下一级的所有节点列表
        /// </summary>
        /// <param name="node">要解析的XML节点</param>
        /// <returns></returns>
        public static XmlNodeList GetNode(XmlNode node)
        {
            if (node == null)
            {
                throw new System.Exception("要解析节点不能为空!");
            }
            return node.ChildNodes;
        }

        /// <summary>
        ///     返回一个XML文件中的所有2级XML节点
        /// </summary>
        /// <param name="filename">XML配置文件</param>
        /// <returns>返回所有2级XML节点</returns>
        public static XmlNodeList GetNode(String filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException("要解析的XML文件未找到!");
                }
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode rootNode = document.DocumentElement;
                if (rootNode != null)
                {
                    return rootNode.ChildNodes;
                }
                return null;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("得到XML节点出现异常 ： " + e.Message);
                return null;
            }
        }

        public static List<InnerXmlNodeInfomation> GetNodes(String xml)
        {
            List<InnerXmlNodeInfomation> nodes = new List<InnerXmlNodeInfomation>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            InnerGetSections(document.ChildNodes, nodes);
            return nodes;
        }

        public static List<InnerXmlNodeInfomation> GetNodes(XmlNodeList nodeList)
        {
            List<InnerXmlNodeInfomation> nodes = new List<InnerXmlNodeInfomation>();
            InnerGetSections(nodeList, nodes);
            return nodes;
        }

        private static void InnerGetSections(XmlNodeList nodeList, List<InnerXmlNodeInfomation> hashs)
        {
            foreach (XmlNode node in nodeList)
            {
                //存在子节点
                if (node.HasChildNodes)
                {
                    InnerGetSections(node.ChildNodes, hashs);
                }
                //默认的第三方自定义节点名称
                if (node.Name != "CustomerConfig" && !node.HasChildNodes)
                {
                    hashs.Add(new InnerXmlNodeInfomation {Name = node.Name, OutputXml = node.OuterXml});
                }
            }
        }

        /// <summary>
        ///     获取一个配置节的值散列HASH
        /// </summary>
        /// <param name="xml">XML文档</param>
        /// <returns>返回值散列HASH</returns>
        public static Dictionary<String,String> GetSectionValues(String xml)
        {
            Dictionary<String,String> hashs = new Dictionary<string, string>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            foreach (XmlNode node in document.ChildNodes)
            {
                //不存在子节点了
                if (node.HasChildNodes)
                {
                    Dictionary<String, String> nodeHashs = GetNodeValues(node.ChildNodes);
                    if (nodeHashs != null && nodeHashs.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in nodeHashs)
                        {
                            hashs.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }
                }
            }
            return hashs;
        }

        /// <summary>
        ///     获取一个配置点的值散列HASH
        /// </summary>
        /// <param name="nodeList">XML节点链表</param>
        /// <returns>返回值散列HASH</returns>
        public static Dictionary<String, String> GetNodeValues(XmlNodeList nodeList)
        {
            Dictionary<String, String> hashs = new Dictionary<string, string>();
            if (nodeList == null)
            {
                return null;
            }
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    hashs.Add(attribute.Name, attribute.InnerText);
                }
            }
            return hashs;
        }

        /// <summary>
        ///     获取一个XML节点内部所有属性的值
        /// </summary>
        /// <param name="xml">XML节点元数据</param>
        /// <returns>返回节点内部所有属性的值</returns>
        public static Dictionary<String, String> GetNodeAttributes(String xml)
        {
            Dictionary<String, String> hashs = new Dictionary<string, string>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            foreach (XmlAttribute attribute in document.ChildNodes[0].Attributes)
            {
                hashs.Add(attribute.Name, attribute.InnerText);
            }
            return hashs;
        }
    }
}
