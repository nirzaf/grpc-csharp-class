using System;
using System.IO;
using Greet;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;

namespace server
{
    internal class Program
    {
        private const int Port = 50051;

        private static void Main(string[] args)
        {
            Server server = null;

            try
            {
                //var serverCert = File.ReadAllText("ssl/server.crt");
                //var serverKey = File.ReadAllText("ssl/server.key");
                //var keypair = new KeyCertificatePair(serverCert, serverKey);
                //var cacert = File.ReadAllText("ssl/ca.crt");

                //var credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, true);

                var reflectionServiceImpl =
                    new ReflectionServiceImpl(GreetingService.Descriptor, ServerReflection.Descriptor);

                server = new Server
                {
                    Services =
                    {
                        GreetingService.BindService(new GreetingServiceImpl()),
                        ServerReflection.BindService(reflectionServiceImpl)
                    },
                    Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)} // credentials) }
                };

                server.Start();
                Console.WriteLine("The server is listening on the port : " + Port);
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine("The server failed to start : " + e.Message);
                throw;
            }
            finally
            {
                if (server != null)
                    server.ShutdownAsync().Wait();
            }
        }
    }
}