using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using Serilog;

namespace SimpleMqttServer
{
    public class Program
    {
        private static Action<MqttServerClientConnectedEventArgs> ClientConnectionHandler => clientConn =>
        {
            //TODO:
            
            Console.WriteLine($"Connected. Client ID: {clientConn.ClientId}");
        };

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "MqttServerLog.txt"),
                rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            var srv = new MqttService();
            srv.Run();
            Console.WriteLine("Server stopped.");
            //ProcessCommands();

            
        }
    }
}