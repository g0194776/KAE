using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using KJFramework.Net.Configurations;
using KJFramework.Logger;
using KJFramework.Basic.Enum;

namespace KJFramework.Net.Helper
{
    /// <summary>
    ///     提供了对于固定信息的相关操作
    /// </summary>
    public static class NetHelper
    {
        #region 成员

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        /// <summary>
        ///     消息体结束标示
        /// </summary>
        public static byte[] MessageBodyEndFlag = Encoding.Default.GetBytes(LocalConfiguration.Current.NetworkLayer.MessageHeaderEndFlag);
        /// <summary>
        ///     消息体结束标示长度
        /// </summary>
        public static int MessageBodyEndFlagLength = LocalConfiguration.Current.NetworkLayer.MessageHeaderEndFlagLength;
        /// <summary>
        ///     消息头标示长度
        /// </summary>
        public static int MessageHeaderFlagLength = LocalConfiguration.Current.NetworkLayer.MessageHeaderFlagLength;
        /// <summary>
        ///     消息头大小
        ///         * 消息头长度计算 = 消息头大小 + 消息头标示长度
        /// </summary>
        public static int MessageHeaderLength = LocalConfiguration.Current.NetworkLayer.MessageHeaderLength + MessageHeaderFlagLength;
        /// <summary>
        ///     消息头标示
        /// </summary>
        public static byte[] MessageHeaderFlag = Encoding.Default.GetBytes(LocalConfiguration.Current.NetworkLayer.MessageHeaderFlag);
        /// <summary>
        ///     消息钩子存放目录
        /// </summary>
        public static String MessageHookFolder = LocalConfiguration.Current.NetworkLayer.MessageHookFolder;
        /// <summary>
        ///     消息拦截器存放目录
        /// </summary>
        public static String MessageSpyFolder = LocalConfiguration.Current.NetworkLayer.SpyFolder;
        /// <summary>
        ///     默认的连接池连接数量上限
        /// </summary>
        public static int DefalutConnectCount = LocalConfiguration.Current.NetworkLayer.DefaultConnectionPoolConnectCount;
        /// <summary>
        ///     服务器性能指标(CPU)
        /// </summary>
        public static float PredominantCpuUsage = LocalConfiguration.Current.NetworkLayer.PredominantCpuUsage;
        /// <summary>
        ///     服务器性能指标(内存)
        /// </summary>
        public static float PredominantMemoryUsage = LocalConfiguration.Current.NetworkLayer.PredominantMemoryUsage;
        /// <summary>
        ///     缓冲区大小
        /// </summary>
        public static int ReadBufferSize = LocalConfiguration.Current.NetworkLayer.BufferSize;
        /// <summary>
        ///     默认的可抛弃类型数据队列的上限值
        /// </summary>
        public static int DefaultDecleardSize = LocalConfiguration.Current.NetworkLayer.DefaultDecleardSize;
        /// <summary>
        ///     默认的缓冲池缓冲上限
        /// </summary>
        public static int DefaultBufferPoolDiscardSize = LocalConfiguration.Current.NetworkLayer.BufferPoolSize;
        /// <summary>
        ///     TCP协议内置保持存活字节码
        /// </summary>
        public static byte[] KeepAliveValue;

        #endregion

        #region 方法

        /// <summary>
        ///     获取本机真实IP地址
        ///         * 如果出现错误则会返回null。
        /// </summary>
        /// <returns>返回本机的真实IP地址</returns>
        public static string GetRealityIP()
        {
            try
            {
                Uri uri = new Uri("http://www.ikaka.com/ip/index.asp");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = 0;
                req.CookieContainer = new CookieContainer();
                req.GetRequestStream().Write(new byte[0], 0, 0);
                HttpWebResponse res = (HttpWebResponse)(req.GetResponse());
                StreamReader rs = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("GB18030"));
                string s = rs.ReadToEnd();
                rs.Close();
                req.Abort();
                res.Close();
                //Match m = Regex.Match(s, @"IP：\[(?<IP>[0-9\.]*)\]");
                int fistposition = s.IndexOf("你的IP：");
                int endposition = s.IndexOf("来自：");
                String[] ipchunk = s.Substring(fistposition, endposition - fistposition).Split('.');
                String ipaddress = ipchunk[0].Substring(ipchunk[0].Length - 3, 3);
                ipaddress += ".";
                ipaddress += ipchunk[1];
                ipaddress += ".";
                ipaddress += ipchunk[2];
                ipaddress += ".";
                ipaddress += ipchunk[3].Substring(0, 3);
                return ipaddress;
            }
            catch (System.Exception e)
            {
                Logs.Logger.Log(e, DebugGrade.Low, Logs.Name);
                return null;
            }
        }

        /// <summary>
        ///     检测当前计算机是否在网络上
        ///         * 此方法更多的是检测当前计算机是否连接这网络设备。
        ///           比如： 网线。
        /// </summary>
        /// <returns>返回连接状态</returns>
        public static bool GetInternetConnectState()
        {
            int i;
            return InternetGetConnectedState(out i, 0);            
        }

        #endregion

        #region 构造函数

        static NetHelper()
        {
            Initialize();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public static void Initialize()
        {
            uint dummy = 0;
            KeepAliveValue = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(KeepAliveValue, 0);//是否启用Keep-Alive
            BitConverter.GetBytes((uint)5000).CopyTo(KeepAliveValue, Marshal.SizeOf(dummy));//多长时间开始第一次探测
            BitConverter.GetBytes((uint)5000).CopyTo(KeepAliveValue, Marshal.SizeOf(dummy) * 2);//探测时间间隔
        }

        #endregion
    }
}
