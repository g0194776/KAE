using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace KJFramework.Net
{
    /// <summary>
    ///     提供了基础的路由UPNP穿透功能
    /// </summary>
    public class UPnP
    {
        #region 成员

        private static TimeSpan _timeout = new TimeSpan(0, 0, 0, 3);
        /// <summary>
        ///     获取或设置超时时间
        /// </summary>
        public static TimeSpan TimeOut
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        ///     描述URL
        /// </summary>
        private static string _descUrl;
        /// <summary>
        ///     服务URL
        /// </summary>
        private static string _serviceUrl;
        /// <summary>
        ///     事件URL
        /// </summary>
        private static string _eventUrl;

        #endregion

        #region 方法

        /// <summary>
        ///     检测当前服务器是否支持UPNP功能
        /// </summary>
        /// <returns>返回false, 则表示不支持</returns>
        public static bool Check()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            string req = "M-SEARCH * HTTP/1.1\r\n" +
                         "HOST: 239.255.255.250:1900\r\n" +
                         "ST:upnp:rootdevice\r\n" +
                         "MAN:\"ssdp:discover\"\r\n" +
                         "MX:3\r\n\r\n";
            byte[] data = Encoding.ASCII.GetBytes(req);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Broadcast, 1900);
            byte[] buffer = new byte[0x1000];

            DateTime start = DateTime.Now;

            do
            {
                socket.SendTo(data, ipe);
                socket.SendTo(data, ipe);
                socket.SendTo(data, ipe);

                int length;
                do
                {
                    try
                    {
                        socket.ReceiveTimeout = 5;
                        length = socket.Receive(buffer);
                        string resp = Encoding.ASCII.GetString(buffer, 0, length).ToLower();
                        if (resp.Contains("upnp:rootdevice"))
                        {
                            resp = resp.Substring(resp.ToLower().IndexOf("location:") + 9);
                            resp = resp.Substring(0, resp.IndexOf("\r")).Trim();
                            if (!string.IsNullOrEmpty(_serviceUrl = GetServiceUrl(resp)))
                            {
                                _descUrl = resp;
                                return true;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.WriteLine("无法映射路由UPNP端口，因为路由没有开启UPNP功能！" + ex.Message);
                        return false;
                    }
                } while (length > 0);
            } while (start.Subtract(DateTime.Now) < _timeout);
            return false;
        }

        /// <summary>
        ///     获取路由器服务地址
        /// </summary>
        /// <param name="resp">请求内容</param>
        /// <returns>返回路由器真实地址</returns>
        private static string GetServiceUrl(string resp)
        {
#if !DEBUG
            try
            {
#endif
            XmlDocument desc = new XmlDocument();
            desc.Load(WebRequest.Create(resp).GetResponse().GetResponseStream());
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
            nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
            XmlNode typen = desc.SelectSingleNode("//tns:device/tns:deviceType/text()", nsMgr);
            if (!typen.Value.Contains("InternetGatewayDevice"))
                return null;
            XmlNode node = desc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:controlURL/text()", nsMgr);
            if (node == null)
                return null;
            XmlNode eventnode = desc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:eventSubURL/text()", nsMgr);
            _eventUrl = CombineUrls(resp, eventnode.Value);
            return CombineUrls(resp, node.Value);
#if !DEBUG
            }
            catch { return null; }
#endif
        }

        private static string CombineUrls(string resp, string p)
        {
            int n = resp.IndexOf("://");
            n = resp.IndexOf('/', n + 3);
            return resp.Substring(0, n) + p;
        }

        /// <summary>
        ///     向路由器中注册UPNP端口
        /// </summary>
        /// <param name="port">本机端口</param>
        /// <param name="routerPort">要映射到的路由器端口</param>
        /// <param name="protocol">协议</param>
        /// <param name="description">描述</param>
        public static void ForwardPort(int port, int routerPort, ProtocolType protocol, string description)
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new System.Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl, "<u:AddPortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                                                        "<NewRemoteHost></NewRemoteHost><NewExternalPort>" + routerPort + "</NewExternalPort><NewProtocol>" + protocol.ToString().ToUpper() + "</NewProtocol>" +
                                                        "<NewInternalPort>" + port + "</NewInternalPort><NewInternalClient>" + Dns.GetHostAddresses(Dns.GetHostName())[0].ToString() +
                                                        "</NewInternalClient><NewEnabled>1</NewEnabled><NewPortMappingDescription>" + description +
                                                        "</NewPortMappingDescription><NewLeaseDuration>0</NewLeaseDuration></u:AddPortMapping>", "AddPortMapping");
        }

        /// <summary>
        ///     删除路由器中的映射端口
        /// </summary>
        /// <param name="port">路由器中的映射端口</param>
        /// <param name="protocol">协议</param>
        public static void DeleteForwardingRule(int port, ProtocolType protocol)
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new System.Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl,
                                           "<u:DeletePortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                                           "<NewRemoteHost>" +
                                           "</NewRemoteHost>" +
                                           "<NewExternalPort>" + port + "</NewExternalPort>" +
                                           "<NewProtocol>" + protocol.ToString().ToUpper() + "</NewProtocol>" +
                                           "</u:DeletePortMapping>", "DeletePortMapping");
        }

        /// <summary>
        ///     得到公网真实地址
        /// </summary>
        /// <returns>返回公网地址</returns>
        public static IPAddress GetExternalIP()
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new System.Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl, "<u:GetExternalIPAddress xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                                                        "</u:GetExternalIPAddress>", "GetExternalIPAddress");
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xdoc.NameTable);
            nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
            string ip = xdoc.SelectSingleNode("//NewExternalIPAddress/text()", nsMgr).Value;
            return IPAddress.Parse(ip);
        }

        /// <summary>
        ///     私有方法，序列化请求URL
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="soap">请求内容</param>
        /// <param name="function">请求功能</param>
        /// <returns></returns>
        private static XmlDocument SOAPRequest(string url, string soap, string function)
        {
            string req = "<?xml version=\"1.0\"?>" +
                         "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                         "<s:Body>" +
                         soap +
                         "</s:Body>" +
                         "</s:Envelope>";
            WebRequest r = HttpWebRequest.Create(url);
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#" + function + "\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }

        #endregion
    }
}