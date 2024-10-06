using Grpc.Core;
using System;
using System.Threading.Tasks;
using MessageService;

namespace BrokerGrpc
{
    public class MessageServiceImpl : MessageService.MessageServiceBase
    {
        // Implementarea metodei SendMessage
        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            Console.WriteLine("Mesaj primit de la sender: " + request.Content);

            // Mesajul poate fi redirecționat către receiver (vezi codul ReceiverGrpc)
            var receiverResponse = ForwardMessageToReceiver(request.Content);

            return Task.FromResult(new MessageResponse { Status = receiverResponse });
        }

        // Metodă pentru redirecționarea mesajului la receiver (RPC către receiver)
        private string ForwardMessageToReceiver(string messageContent)
        {
            // Creare canal gRPC către receiver
            var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new MessageService.MessageServiceClient(channel);

            // Trimiterea mesajului către receiver
            var request = new MessageRequest { Content = messageContent };
            var reply = client.SendMessage(request);

            Console.WriteLine("Mesaj redirecționat către receiver: " + messageContent);
            return reply.Status;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int Port = 5001;

            // Creare server gRPC pentru a asculta mesajele de la sender
            Server server = new Server
            {
                Services = { MessageService.BindService(new MessageServiceImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();
            Console.WriteLine($"Broker gRPC pornit pe portul {Port}");
            Console.ReadLine();
            server.ShutdownAsync().Wait();
        }
    }
}
