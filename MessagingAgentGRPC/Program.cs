using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using MessageService;

namespace SenderGrpc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Creare canal gRPC către broker
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new MessageService.MessageServiceClient(channel);

            // Trimiterea mesajului către broker
            var request = new MessageRequest { Content = "Salut, receiver!" };
            var reply = await client.SendMessageAsync(request);

            Console.WriteLine("Răspuns de la broker: " + reply.Status);
        }
    }
}
