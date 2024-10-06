using Grpc.Core;
using System;
using System.Threading.Tasks;
using MessageService;

namespace ReceiverGrpc
{
    public class MessageServiceImpl : MessageService.MessageServiceBase
    {
        // Implementarea metodei SendMessage pentru a primi mesajul de la broker
        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            Console.WriteLine("Mesaj primit de la broker: " + request.Content);

            // Răspunsul către broker
            return Task.FromResult(new MessageResponse { Status = "Mesaj primit cu succes!" });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int Port = 5002;

            // Creare server gRPC pentru a asculta mesajele de la broker
            Server server = new Server
            {
                Services = { MessageService.BindService(new MessageServiceImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();
            Console.WriteLine($"Receiver gRPC pornit pe portul {Port}");
            Console.ReadLine();
            server.ShutdownAsync().Wait();
        }
    }
}
