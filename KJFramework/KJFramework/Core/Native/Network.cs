using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace KJFramework.Core.Native
{
    /// <summary>
    ///     KJFramework底层调用核心
    ///         * 更多的是对于系统底层API的封装。
    /// </summary>
    public partial class Native
    {
        /// <summary>
        ///     封装了对于系统底层通讯的API调用。
        /// </summary>
        public class Network
        {
            #region 声明

            [StructLayout(LayoutKind.Sequential)]
            public class MIB_TCPROW
            {
                public int dwState;
                public int dwLocalAddr;
                public int dwLocalPort;
                public int dwRemoteAddr;
                public int dwRemotePort;
            }

            [StructLayout(LayoutKind.Sequential)]
            public class MIB_TCPTABLE
            {
                public int dwNumEntries;
                public MIB_TCPROW[] table;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_UDPSTATS
            {
                public int dwInDatagrams;
                public int dwNoPorts;
                public int dwInErrors;
                public int dwOutDatagrams;
                public int dwNumAddrs;
            }

            [StructLayout(LayoutKind.Sequential)]
            public class MIB_UDPTABLE
            {
                public int dwNumEntries;
                public MIB_UDPROW[] table;

            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_UDPROW
            {
                public String Local;
            }

            public struct MIB_EXUDPTABLE
            {
                public int dwNumEntries;
                public MIB_EXUDPROW[] table;

            }

            public struct MIB_EXUDPROW
            {
                public IPEndPoint Local;
                public int dwProcessId;
                public string ProcessName;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_TCPSTATS
            {
                public int dwRtoAlgorithm;
                public int dwRtoMin;
                public int dwRtoMax;
                public int dwMaxConn;
                public int dwActiveOpens;
                public int dwPassiveOpens;
                public int dwAttemptFails;
                public int dwEstabResets;
                public int dwCurrEstab;
                public int dwInSegs;
                public int dwOutSegs;
                public int dwRetransSegs;
                public int dwInErrs;
                public int dwOutRsts;
                public int dwNumConns;
            }
            
            public struct MIB_EXTCPTABLE
            {
                public int dwNumEntries;
                public MIB_EXTCPROW[] table;

            }
            public struct MIB_EXTCPROW
            {
                public string StrgState;
                public int iState;
                public IPEndPoint Local;
                public IPEndPoint Remote;
                public int dwProcessId;
                public string ProcessName;
            }


            [DllImport("Iphlpapi.dll")]
            static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

            [DllImport("Iphlpapi.dll")]
            static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);

            [DllImport("Ws2_32.dll")]
            static extern Int32 inet_addr(string ipaddr);

            [DllImport("Ws2_32.dll")]
            static extern ushort ntohs(ushort netshort);

            [DllImport("iphlpapi.dll", SetLastError = true)]
            public extern static int GetTcpStatistics(ref MIB_TCPSTATS pStats);

            [DllImport("iphlpapi.dll", SetLastError = true)]
            public static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder);

            [DllImport("iphlpapi.dll", SetLastError = true)]
            public extern static int AllocateAndGetTcpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

            [DllImport("iphlpapi.dll", SetLastError = true)]
            public extern static int AllocateAndGetUdpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

            [DllImport("kernel32", SetLastError = true)]
            public static extern IntPtr GetProcessHeap();

            [DllImport("kernel32", SetLastError = true)]
            private static extern int FormatMessage(int flags, IntPtr source, int messageId,
             int languageId, StringBuilder buffer, int size, IntPtr arguments);



            #endregion

#if !MONO

            #region 方法

            //SendArp获取MAC地址
            public static string GetMacAddress(string macip)
            {
                StringBuilder strReturn = new StringBuilder();
                try
                {
                    Int32 remote = inet_addr(macip);

                    Int64 macinfo = new Int64();
                    Int32 length = 6;
                    SendARP(remote, 0, ref macinfo, ref length);

                    string temp = System.Convert.ToString(macinfo, 16).PadLeft(12, '0').ToUpper();

                    int x = 12;
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 5) { strReturn.Append(temp.Substring(x - 2, 2)); }
                        else { strReturn.Append(temp.Substring(x - 2, 2) + ":"); }
                        x -= 2;
                    }

                    return strReturn.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            public static bool IsHostAlive(string strHostIP)
            {
                string strHostMac = GetMacAddress(strHostIP);
                return !string.IsNullOrEmpty(strHostMac);
            }

            public static MIB_TCPTABLE GetTcpTableInfo()
            {
                //声明一个指针准备接受Tcp连接信息
                IntPtr hTcpTableData = IntPtr.Zero;

                //声明hTcpTableData指针所指向的内存缓冲区大小
                int iBufferSize = 0;

                //声明MIB_TCPTABLE对象，作为返回值
                MIB_TCPTABLE tcpTable = new MIB_TCPTABLE();

                //声明一个List对象来临时存放MIB_TCPROW对象
                List<MIB_TCPROW> lstTcpRows = new List<MIB_TCPROW>();

                //调用API来获得真正的缓冲区大小，iBufferSize默认为0，
                //这时调用API GetTcpTable会触发一个异常ERROR_INSUFFICIENT_BUFFER
                //通过这个异常系统会把真正的缓冲长度返回
                GetTcpTable(hTcpTableData, ref iBufferSize, false);

                //为托管指针在堆上分配内存
                hTcpTableData = Marshal.AllocHGlobal(iBufferSize);

                //求得MIB_TCPROW对象的内存字节数
                int iTcpRowLen = Marshal.SizeOf(typeof(MIB_TCPROW));

                //根据上面得到的缓冲区大小来推算MIB_TCPTABLE里的MIB_TCPROW数组长度
                //下面用缓冲长度-sizeof(int)也就是去掉MIB_TCPTABLE里的成员dwNumEntries所占用的内存字节数
                int aryTcpRowLength = (int)Math.Ceiling((double)(iBufferSize - sizeof(int)) / iTcpRowLen);

                //重新取得TcpTable的数据
                GetTcpTable(hTcpTableData, ref iBufferSize, false);

                //下面是关键，由于MIB_TCPTABLE里的成员有一个是数组，而这个数组长度起初我们是不能确定的
                //所以这里我们只能根据分配的指针来进行一些运算来推算出我们所要的数据
                for (int i = 0; i < aryTcpRowLength; i++)
                {
                    //hTcpTableData是指向MIB_TCPTABLE缓冲区的内存起始区域，由于其成员数据在内存中是顺序排列
                    //所以我们可以推断hTcpTableData+4(也就是sizeof(dwNumEntries)的长度)后就是MIB_TCPROW数组的第一个元素
                    IntPtr hTempTableRow = new IntPtr(hTcpTableData.ToInt32() + 4 + i * iTcpRowLen);
                    MIB_TCPROW tcpRow = new MIB_TCPROW();
                    tcpRow.dwLocalAddr = 0;
                    tcpRow.dwLocalPort = 0;
                    tcpRow.dwRemoteAddr = 0;
                    tcpRow.dwRemotePort = 0;
                    tcpRow.dwState = 0;

                    //把指针数据拷贝到我们的结构对象里。
                    Marshal.PtrToStructure(hTempTableRow, tcpRow);
                    lstTcpRows.Add(tcpRow);
                }

                tcpTable.dwNumEntries = lstTcpRows.Count;
                tcpTable.table = new MIB_TCPROW[lstTcpRows.Count];
                lstTcpRows.CopyTo(tcpTable.table);
                return tcpTable;
            }

            public static string GetIpAddress(long ipAddrs)
            {
                try
                {
                    IPAddress ipAddress = new IPAddress(ipAddrs);
                    return ipAddress.ToString();
                }
                catch { return ipAddrs.ToString(); }

            }

            public static ushort GetTcpPort(int tcpPort)
            {
                return ntohs((ushort)tcpPort);
            }

            public static bool IsPortBusy(int port)
            {
                MIB_TCPTABLE tcpTableData = GetTcpTableInfo();
                return false;
            }

            /// <summary>
            ///     返回当前所有处于监听状态的TCP端口列表
            /// </summary>
            /// <returns>返回一个数组集合</returns>
            public static ArrayList GetTcpOpenPort()
            {
                MIB_TCPTABLE Table = GetTcpTableInfo();
                ArrayList list = new ArrayList();
                for (int i = 0; i < Table.dwNumEntries; i++)
                {
                    if ((Table.table[i].dwLocalAddr == Table.table[i].dwRemoteAddr) && (Table.table[i].dwLocalAddr == 0 && Table.table[i].dwRemoteAddr == 0))
                    {
                        if (GetTcpPort(Table.table[i].dwLocalPort) != 0)
                        {
                            list.Add(GetTcpPort(Table.table[i].dwLocalPort));
                        }
                    }
                }
                return list;
            }

            /// <summary>
            ///     返回当前所有处于监听状态的UDP端口列表
            /// </summary>
            /// <returns>返回一个数组集合</returns>
            public static ArrayList GetUdpOpenPort()
            {
                ArrayList arrayList = new ArrayList();
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                foreach (IPEndPoint endPoint in ipGlobalProperties.GetActiveUdpListeners())
                {
                    if (endPoint.Address.ToString() == "0.0.0.0")
                    {
                        arrayList.Add(endPoint.Port);
                    }
                }
                return arrayList;
            }

            #endregion

#endif
        }
    }
}
