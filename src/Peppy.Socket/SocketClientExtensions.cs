using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.Socket
{
    public static class SocketClientExtensions
    {
        public static void Accept(this SocketClient socketClient, Action callback)
        {
            try
            {
                //这就是客户端的Socket实例，我们后续可以将其保存起来
                var socket = socketClient.Socket;
                socket.BeginAccept(ar =>
                {
                    var client = socket.EndAccept(ar);
                    callback?.Invoke();
                    socketClient.AcceptSocket = client;
                    //准备接受下一个客户端请求
                    Accept(socketClient, callback);
                }, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void Receive(this SocketClient socketClient, Action<string> received)
        {
            if (socketClient.AcceptSocket == null) return;
            try
            {
                ReceiveCallback(socketClient, received);
            }
            catch (Exception ex)
            {
                if (!socketClient.AcceptSocket.Connected)
                    Accept(socketClient, () =>
                    {
                        ReceiveCallback(socketClient, received);
                    });
            }
        }

        public static async Task ReceiveAsync(this SocketClient socketClient, Action<string> received)
            => await Task.Run(() => { socketClient.Receive(received); });

        private static void ReceiveCallback(SocketClient socketClient, Action<string> received)
        {
            var buffer = new byte[1048];
            // 接收消息
            var receiveNumber = socketClient.AcceptSocket.Receive(buffer);
            if (receiveNumber <= 0) return;
            buffer = buffer.Skip(0).Take(receiveNumber).ToArray();
            var result = HexHelper.ByteToHexStr(buffer);
            received(result);
        }

        public static void SendMessage(this SocketClient socketClient, string message)
        {
            if (socketClient.AcceptSocket == null) return;
            var data = HexHelper.HexStrToByte(message);
            try
            {
                socketClient.AcceptSocket.Send(data);
            }
            catch (Exception ex)
            {
                if (!socketClient.AcceptSocket.Connected)
                    Accept(socketClient, () =>
                    {
                        socketClient.AcceptSocket.Send(data);
                    });
            }
        }

        public static async Task SendMessageAsync(this SocketClient socketClient, string message)
            => await Task.Run(() => { socketClient.SendMessage(message); });

        public static void SendMessages(this SocketClient socketClient, List<string> messages)
        {
            foreach (var message in messages)
            {
                socketClient.SendMessage(message);
            }
        }

        private static async Task SendMessagesAsync(this SocketClient socketClient, List<string> messages)
            => await Task.Run(() => { socketClient.SendMessages(messages); });
    }
}