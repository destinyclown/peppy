using Grpc.Core;
using Grpc.Net.Client;
using Peppy.GrpcService;
using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Peppy.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:50001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "GrpcClient" });

            Console.WriteLine(reply.Message);
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}