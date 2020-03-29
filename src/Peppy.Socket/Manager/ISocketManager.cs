using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Socket.Manager
{
    public interface ISocketManager
    {
        /// <summary>
        /// 获取Socket客户端列表
        /// </summary>
        /// <returns></returns>
        List<SocketClient> GetSocketClients();

        /// <summary>
        /// 获取Socket客户端
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        SocketClient GetSocketClient(string fullName);
    }
}