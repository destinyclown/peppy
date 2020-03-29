using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Peppy.Socket.Manager;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Peppy.Socket
{
    public class SocketManager : ISocketManager
    {
        private static readonly ConcurrentQueue<SocketClient> SocketClients = new ConcurrentQueue<SocketClient>();

        public SocketManager(
            ILogger logger,
            IOptions<SocketOptions> options)
        {
            foreach (var socketAddress in options.Value.SocketAddresses)
            {
                var ipAddress = IPAddress.Parse(socketAddress.HostName);
                var ipEndPoint = new IPEndPoint(ipAddress, socketAddress.Port);
                //创建服务器Socket对象，并设置相关属性
                var socket = new System.Net.Sockets.Socket(
                    socketAddress.AddressFamily,
                    socketAddress.SocketType,
                    socketAddress.ProtocolType
                    );
                //绑定ip和端口
                socket.Bind(ipEndPoint);
                //设置最长的连接请求队列长度
                socket.Listen(socketAddress.MaxBacklog);
                var clientSocket = new SocketClient()
                {
                    Socket = socket,
                    SocketAddress = socketAddress
                };
                SocketClients.Enqueue(clientSocket);
                logger.LogInformation($"Socket:" +
                                      $"{socketAddress.FullName} " +
                                      $"{socketAddress.HostName}:" +
                                      $"{socketAddress.Port} 注册监听成功");
            }
        }

        /// <summary>
        /// 获取Socket客户端列表
        /// </summary>
        /// <returns></returns>
        public List<SocketClient> GetSocketClients()
            => SocketClients.ToList();

        /// <summary>
        /// 获取Socket客户端
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public SocketClient GetSocketClient(string fullName)
            => SocketClients.FirstOrDefault(x => x.SocketAddress.FullName.Equals(fullName));
    }
}