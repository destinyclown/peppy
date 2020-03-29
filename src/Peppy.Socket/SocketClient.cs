using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Socket
{
    public class SocketClient
    {
        /// <summary>
        /// Socket对象
        /// </summary>
        public System.Net.Sockets.Socket Socket { get; set; }

        /// <summary>
        /// Socket操作对象
        /// </summary>
        public System.Net.Sockets.Socket AcceptSocket { get; set; }

        /// <summary>
        /// Socket客户端地址
        /// </summary>
        public SocketAddress SocketAddress { get; set; }
    }
}