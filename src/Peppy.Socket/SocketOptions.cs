using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Peppy.Socket
{
    /// <summary>
    /// Socket注册信息
    /// </summary>
    public class SocketOptions
    {
        /// <summary>
        /// Socket地址信息集合
        /// </summary>
        public List<SocketAddress> SocketAddresses { get; set; }
    }

    /// <summary>
    /// Socket地址信息
    /// </summary>
    public class SocketAddress
    {
        public SocketAddress(
            string fullName,
            string hostName,
            int port,
            int maxBacklog,
            AddressFamily addressFamily = AddressFamily.InterNetwork,
            SocketType socketType = SocketType.Stream,
            ProtocolType protocolType = ProtocolType.Tcp)
        {
            FullName = fullName;
            HostName = hostName;
            Port = port;
            MaxBacklog = maxBacklog;
            AddressFamily = addressFamily;
            SocketType = socketType;
            ProtocolType = protocolType;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 主机ip
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 队列最大积压
        /// </summary>
        public int MaxBacklog { get; set; }

        /// <summary>
        /// 地址族
        /// </summary>
        public AddressFamily AddressFamily { get; set; }

        /// <summary>
        /// Socket类型
        /// </summary>
        public SocketType SocketType { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
    }
}