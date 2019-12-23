using DotNetCore.CAP;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Peppy.GrpcService;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Peppy.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //var channel = GrpcChannel.ForAddress("http://localhost:50001");
            //var client = new Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GrpcClient" });

            //Console.WriteLine(reply.Message);
            //Console.WriteLine("Greeting: " + reply.Message);
            //Console.WriteLine("Press any key to exit...");
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ISubscriberService, SubscriberService>();

                    services.AddCap(x =>
                    {
                        x.UseMySql("Server=192.168.6.45;Database=captest;UserId=root;Password=#7kfnymAM$Y9-Ntf;port=3306;Convert Zero Datetime=True;allowPublicKeyRetrieval=true");
                        x.UseRabbitMQ(configure =>
                        {
                            configure.HostName = "134.175.159.22";
                            configure.UserName = "bailun";
                            configure.Password = "bailun2019";
                        });
                        x.FailedRetryCount = 5;
                    });
                });

            await builder.RunConsoleAsync();
        }

        public interface ISubscriberService
        {
            public void CheckReceivedMessage(DateTime datetime);
        }

        public class SubscriberService : ISubscriberService, ICapSubscribe
        {
            [CapSubscribe("sample.rabbitmq.qq")]
            public void CheckReceivedMessage(DateTime datetime)
            {
                //Thread.Sleep(5000);
                throw new Exception("test");
                // Console.WriteLine(datetime);
            }
        }
    }
}